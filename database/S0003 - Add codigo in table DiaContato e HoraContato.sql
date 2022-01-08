/*
 * S0003 - Add codigo in table DiaContato e HoraContato
 */

ALTER TABLE ClienteDiaContato
    ADD COLUMN codigo BIGSERIAL PRIMARY KEY;
	
ALTER TABLE ClienteHoraContato
    ADD COLUMN codigo BIGSERIAL PRIMARY KEY;