
# pip install psycopg2

import psycopg2

try:
    connection = psycopg2.connect(
        host='localhost',
        port=5432,
        database='my_db',
        user='postgres',
        password='1234'
    )

    cursor = connection.cursor()

    with open('./export.sql', 'r') as file:
        sql_script = file.read()

    cursor.execute(sql_script)
    connection.commit()

    print("SQL script executed successfully.")

except psycopg2.Error as e:
    print("Error executing SQL script:", e)

finally:
    if connection:
        cursor.close()
        connection.close()
