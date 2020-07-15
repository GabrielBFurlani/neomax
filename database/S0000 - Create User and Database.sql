/*
 * S0000 - Create User and Database
 */

/*!!!!!! A execução desse script deve ser feita instrução a instrução separadamente !!!!!!*/
 
CREATE USER neomaxuser WITH ENCRYPTED PASSWORD 'neomaxpass';

CREATE DATABASE Neomex;

GRANT ALL PRIVILEGES ON DATABASE Neomex TO neomaxuser;

ALTER DATABASE Neomex OWNER TO neomaxuser;