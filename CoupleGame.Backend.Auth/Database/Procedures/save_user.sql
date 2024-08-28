CREATE OR REPLACE FUNCTION save_user(
    p_name varchar,
    p_email varchar,
    p_externalid varchar
)
RETURNS uuid AS $$
DECLARE
    new_id uuid;
BEGIN
    INSERT INTO users (id, name, email, externalid)
    VALUES (gen_random_uuid(), p_name, p_email, p_externalid)
    RETURNING id INTO new_id;

    RETURN new_id;
END;
$$ LANGUAGE plpgsql;
