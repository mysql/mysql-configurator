# Copyright (c) 2016, 2020, Oracle and/or its affiliates.
# Uninstall firewall tables, functions and procedures.
USE mysql;
DELIMITER //
CREATE PROCEDURE uninstall_enterprise_firewall()
BEGIN
set @m1 = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.PLUGINS WHERE PLUGIN_NAME = 'MYSQL_FIREWALL_USERS');
set @m2 = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.PLUGINS WHERE PLUGIN_NAME = 'MYSQL_FIREWALL_WHITELIST');
set @m3 = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.PLUGINS WHERE PLUGIN_NAME = 'MYSQL_FIREWALL');
IF @m1 >= 1 THEN
  UNINSTALL PLUGIN mysql_firewall_users;
END IF;
IF @m2 >= 1 THEN
  UNINSTALL PLUGIN mysql_firewall_whitelist;
END IF;
IF @m3 >= 1 THEN
  UNINSTALL PLUGIN mysql_firewall;
END IF;
END //
DELIMITER ;
CALL uninstall_enterprise_firewall();
DROP PROCEDURE IF EXISTS uninstall_enterprise_firewall;