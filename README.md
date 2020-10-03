# ExpenseManager
Запуск ServerApp:
1. Перейти к корню репозитория (где лежит файл docker-comppose.yml)
2. Выполнять следующий набор команд:  
   docker-compose pull (только при первом запуске)  
   docker-compose build (только при первом запуске и после каждого rebase)     
   docker-compose up (при каждом запуске)     
3. ServerApp разворачивается по адресу localhost:5000
