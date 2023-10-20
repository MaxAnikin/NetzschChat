# NetzschChat
This is a basic chat solution created as a preinterview task for Netzsch.

Since the task description says that I am free to apply my personal assumptions I decided to implement a chat system.

Solution consists from the following applications:
 
 - Netzsch.Api: a REST api service 
 - Netzsch.Wpf: a WPF client
 - Netzsch.Angular: an Angular client

Application data is stored in the LiteDb database. It is a simple Json storage.

Build and Run:

1. Start rest api service using "Netzsch.Api" folder terminal window:  "dotnet run --launch-profile https | start chrome https://localhost:7147/swagger" 
or just "dotnet run" and open https://localhost:7147/swagger in order to make sure api service is up and running.
      
2. Start wpf client from the "Netzsch.Api" folder terminal window with command: "dotnet run"

3. Start angular application from the "Netzsch.Api" folder terminal window with command: "ng serve"

4. Use registration forms and create users "user1" and "user2". You can use the same value for all the fields. 

5. Start messaging.

Thank you for your task and your time. 




