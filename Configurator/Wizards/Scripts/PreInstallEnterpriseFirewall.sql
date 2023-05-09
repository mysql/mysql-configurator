# Copyright (c) 2016 Oracle and/or its affiliates.
# Installs required firewall tables before starting up the server.
USE mysql;

CREATE TABLE IF NOT EXISTS mysql.firewall_whitelist( USERHOST VARCHAR(80) NOT NULL, RULE text NOT NULL) engine= MyISAM;
CREATE TABLE IF NOT EXISTS mysql.firewall_users( USERHOST VARCHAR(80) PRIMARY KEY, MODE ENUM ('OFF', 'RECORDING', 'PROTECTING', 'RESET', 'DETECTING') DEFAULT 'OFF') engine= MyISAM;
