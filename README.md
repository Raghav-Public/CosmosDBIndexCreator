# Intro

This app will help you fix _id index discrepancies.

The right way to drop all default indexes and recreate individual indexes is:

```
db.collection.dropIndexes();
db.collection.createIndex({<col>:1});
```

To use this app, go to app.config and update
```
<add key="EndPointUrl" value="XXX"/>      
<add key="AuthorizationKey" value="XXX"/>
<add key="DatabaseId" value="mydb"/>
<add key="CollectionId" value="mycol"/>

```

You can also build a docker image and use that to run this app.