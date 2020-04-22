FROM mongo

COPY init.json /init.json
CMD mongoimport --host mongo --db test --collection people --drop --type json --file /init.json --jsonArray