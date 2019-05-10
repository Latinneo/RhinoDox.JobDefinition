# RhinoDox.JobDefinition
Domain service that monitor job definition staging paths for dropped zip files

Setup

This solution uses submodules, which you will need to initialize when you get started. To do so, from the solution directory, run:

git submodule init

git submodule foreach git pull origin master

git submodule update --remote

This will fill out the populate the RhinoDox.JobDefinition.Proto solution into the Contracts folder of the RhinoDox.JobDefinition.Domain project; this is where you will find the classes for commands and events.

----------------------

You will also need to run both Redis and Postgres in Docker. To do so, open up a terminal and run:

[For Redis]

docker pull redis

docker run --name redis -d -p 6379:6379 redis

[For Postgres]

docker pull postgres

docker run -d -p 5432:5432 --name postgres -e POSTGRES_PASSWORD=postgres postgres

Each of these will create a container (named redis or postgres), open the ports of those containers to your machine, and lastly select the image to run inside of those containers (redis and postgres).

Once you run these once, you will only need to run 'docker run [container name]' to get them going.

If you run into an error that indicates those ports have already been assigned to, you can change which localhost port you bind to the container. I ran into this with postgres, and so assigned it to 5431 instead of 5432 (change the first instance of the number; the ports are listed [local port:docker port]. If you do change this, make sure to change the listed port in the appsettings file in the RhinoDox.JobDefinition solution.

---------------------

You do not need, but will probably want, to install pgAdmin4 as well, to better work with Postgres. You can find instructions for setting up pgAdmin4 in Confluence, tied to the AuditDB solution. You do not need to set anything up within pgAdmin once you have it installed; running the RhinoDox.JobDefinition solution, NHibernate will create a database and populate it with the necessary schemas and tables.
