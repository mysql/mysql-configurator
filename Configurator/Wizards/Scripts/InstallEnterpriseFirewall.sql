# Copyright (c) 2016, 2019, Oracle and/or its affiliates.
# Installs required db objects for enterprise firewall after the server has started up.
USE mysql;

CREATE FUNCTION set_firewall_mode RETURNS STRING SONAME 'firewall.dll';
CREATE FUNCTION normalize_statement RETURNS STRING SONAME 'firewall.dll';
CREATE FUNCTION mysql_firewall_flush_status RETURNS STRING SONAME 'firewall.dll';
CREATE AGGREGATE FUNCTION read_firewall_whitelist RETURNS STRING SONAME 'firewall.dll';
CREATE AGGREGATE FUNCTION read_firewall_users RETURNS STRING SONAME 'firewall.dll';

DELIMITER //
CREATE PROCEDURE sp_set_firewall_mode (IN arg_userhost VARCHAR(80), IN arg_mode varchar(12))
BEGIN
DECLARE result VARCHAR(160);
IF arg_mode = "RECORDING" THEN
  SELECT read_firewall_whitelist(arg_userhost,FW.rule) FROM mysql.firewall_whitelist FW WHERE userhost = arg_userhost;
END IF;
SELECT set_firewall_mode(arg_userhost, arg_mode) INTO result;
IF arg_mode = "RESET" THEN
  SET arg_mode = "OFF";
END IF;
IF result = "OK" THEN
  INSERT IGNORE INTO mysql.firewall_users VALUES (arg_userhost, arg_mode);
  UPDATE mysql.firewall_users SET mode=arg_mode WHERE userhost = arg_userhost;
ELSE
  SELECT result;
END IF;
IF arg_mode = "PROTECTING" OR arg_mode = "OFF" OR arg_mode = "DETECTING" THEN
  DELETE FROM mysql.firewall_whitelist WHERE USERHOST = arg_userhost;
  INSERT INTO mysql.firewall_whitelist(USERHOST, RULE) SELECT USERHOST,RULE FROM INFORMATION_SCHEMA.mysql_firewall_whitelist WHERE USERHOST=arg_userhost;
END IF;
END //
CREATE PROCEDURE sp_reload_firewall_rules(IN arg_userhost VARCHAR(80))
BEGIN
  SELECT set_firewall_mode(arg_userhost, "RESET") AS 'Result';
  SELECT read_firewall_whitelist(arg_userhost,FW.rule) AS 'Result' FROM mysql.firewall_whitelist FW WHERE FW.userhost=arg_userhost;
END //
DELIMITER ;