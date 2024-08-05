import psycopg2
import pandas as pd
import redis

from conf import *

COUNTRY = input('Country code: ').upper()

print('Connecting to redis')
r = redis.Redis(
  host=REDIS_HOST,
  port=REDIS_PORT,
  db=REDIS_DB,
  username=REDIS_USER,
  password=REDIS_PASSWORD
)

print(f'Reading {CITIES_FILE_PATH}')
df = pd.read_excel(CITIES_FILE_PATH, usecols=['city_ascii', 'iso2'])
sql_str = 'INSERT INTO cities(name, country)  VALUES '

print('Processing rows')
for index, row in df.iterrows():
  code = row['iso2']
  city = row['city_ascii']

  if code == COUNTRY:
    city = str(city).replace("'", "''").replace('"', '""')
    sql_str += f"('{city}', '{code}'),"

sql_str = sql_str[:-1] + ' ON CONFLICT DO NOTHING;'

print(f'Connecting to database')
conn = psycopg2.connect(
  database=DB_NAME,
  host=DB_HOST,
  user=DB_USER,
  password=DB_PASSWORD,
  port=DB_PORT
)
cursor = conn.cursor()

print('Inserting into database')
print(sql_str[:300] + '...')
cursor.execute(sql_str)
cursor.execute(f"UPDATE countries SET is_supported = TRUE WHERE code = '{COUNTRY}';")
conn.commit()
conn.close()

print('Invalidating redis cache')
r.delete('cities')
r.delete('entities-count')
r.delete('countries')
r.close()

print('Done')
