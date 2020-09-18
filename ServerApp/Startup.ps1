docker stop server
docker rm server
docker rmi expense_manager_server
docker build -t expense_manager_server .
docker run -d -p 5000:80 --name server expense_manager_server
docker rmi $(docker images -f "dangling=true" -q)