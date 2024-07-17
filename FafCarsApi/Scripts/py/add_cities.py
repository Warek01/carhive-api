import psycopg2
import pandas as pd

COUNTRY = input('Country code: ')
HOST = input('Host (localhost): ') or 'localhost'
PORT = input('Port (5432): ') or '5432'
DATABASE = input('Database (faf_cars): ') or 'faf_cars'
USER = input('User (warek): ') or 'warek'
PASSWORD = input('Password (warek): ') or 'warek'
FILE = input('File (../../Resources/worldcities.xlsx): ') or '../../Resources/worldcities.xlsx'

print(f'Reading {FILE}')
df = pd.read_excel(FILE, usecols=['city_ascii', 'iso2'])
sql_str = 'INSERT INTO cities(name, country)  VALUES '

print('Processing rows')
for index, row in df.iterrows():
  code = row['iso2']
  city = row['city_ascii']

  if code == COUNTRY:
    sql_str += f"('{city}', '{code}'),"

sql_str = sql_str[:-1] + ' ON CONFLICT DO NOTHING;'

print(f'Connecting to {HOST}:{PORT}')
conn = psycopg2.connect(
  database=DATABASE,
  host=HOST,
  user=USER,
  password=PASSWORD,
  port=PORT
)
cursor = conn.cursor()

print('Inserting into database')
print(sql_str[:300] + '...')

cursor.execute(sql_str)
conn.commit()
conn.close()
print('Done')
