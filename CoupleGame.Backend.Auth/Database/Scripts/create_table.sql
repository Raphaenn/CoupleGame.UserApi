CREATE TABLE users (
    id uuid NOT NULL,
    name varchar(255) NOT NULL,
    email varchar(255) NOT NULL UNIQUE,
    password varchar(255) NOT NULL,
    birthdate timestamp(0) without time zone NOT NULL,
    externalId varchar(255) NOT NULL,
    PRIMARY KEY (id)
);
