# Uppgiftsapp – backend (ASP.NET Web API)

API för uppgifter. Används av webbappen: https://github.com/SemirZahirovic97/uppgiftsapp-webb

## Starta
```
dotnet run --launch-profile http
```
API:et körs på http://localhost:5005. Testa: http://localhost:5005/api/tasks

## Endpoints
- `GET /api/tasks` – lista uppgifter
- `POST /api/tasks` – skapa uppgift
- `PUT /api/tasks/{id}` – ändra uppgift
- `POST /api/tasks/{id}/image` – ladda upp bild (fältet heter `file`)

## Tekniska val
- **Ingen databas:** uppgifterna ligger i en lista i minnet och nollställs vid omstart, eftersom kraven inte kräver att data sparas.
- **Bilder sparas i `wwwroot/uploads`:** uppgiften sparar bara adressen. Filerna får unika namn, och bara bildfiler tillåts.
- **CORS:** tillåter bara http://localhost:5173.