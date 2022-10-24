I dette prosjektet finner vi følgende:


1. Node prosjekt med web grensesnitt, som poster til topic på ActiveMQ
   -------------------------------------------------------------------
   Pass på at autostart (CMD) er kommentert ut, da starter ikke applikasjonen automatisk
   Attatch til containeren og kjør følgende:
   >cd /source
   >nmp init
   Da kan du kjøre debug av serveren og sette breakpoint
   Gå til http://localhost:3085/index.html
   Fyll ut form ok klikk på knapp.
   Nå skal du kunne stoppe i breakpoint, og debugge
   Gå til http://localhost:8161/admin (admin for ActiveMQ, 
   bruker og passord har du satt i compose filen (admin/admin)
   Velg queues fra menyen, og du skal finne meldingen du la ut på køen
   

2. C# Webjob som trigges av meldinger på køen og legger data i databasen
   Om du får problemer med dette (initialisering av C# extension), så kjør 
   dotnet restore
   dotnet build 
   av prosjektet, da kommer den sikkert
3. Web prosjekt som viser data fra databasen