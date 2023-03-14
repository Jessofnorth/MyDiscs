# DT191G, Projekt - Webbutveckling med .NET

Detta projekt är ett MVC projekt skapat i ASP .NET med Entity Framework samt Identity. Databasen är i SQLite. Dett finns även ett API som går att konsumera från utanför projektet med CRUD funktionalitet. CSS ramverk är Bootstrap. 

En admin del är scaffoldad genom .NET MVC som har Identity för hantering av användare och inlogg, denna del kan ej kommas åt utan att vara inloggad. 

## Länk till live demo 
[Demo](https://jess-mydiscs.azurewebsites.net/)

Nedan beskrivs hur APIet är strukturerat samt vilka endpoints som finns att tillgå. 

## Databastabeller

| Tabell-namn | Fält                                                                                                                                                                                                                        |
| ----------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Disc        | **DiscId** (PK, int), **Name** (string), **Speed** (int), **Glide** (int), **Turn** (int), **Fade** (int), **Plastic** (string), **Bagged** (bool), **ImageName** (string), **CategoryId** (FK, int), **BrandId** (FK, int) |
| Category    | **CategoryId** (PK, int), **CategoryName** (string)                                                                                                                                                                         |
| Brand       | **BrandId** (PK, int), **BrandName** (string)                                                                                                                                                                               |

## Användning av API

## Disc

| Metod  | Ändpunkt  | Beskrivning                                                                            |
| ------ | --------- | -------------------------------------------------------------------------------------- |
| GET    | /api/disc | Hämtar alla tillgängliga discar.                                                       |
| GET    | /api/disc/{id} | Hämtar en specifik disc med angivet ID.                                                |
| POST   | /api/disc | Lagrar en ny disc. Kräver att ett disc-objekt skickas med.                             |
| PUT    | /api/disc/{id} | Uppdaterar en existerande disc med angivet ID. Kräver att ett disc-objekt skickas med. |
| DELETE | /api/disc/{id} | Raderar en disc med angivet ID.                                                        |

Ett disc-objekt returneras/skickas som JSON med följande struktur:

```
{
    "discId": 1,
    "name": "Mako 3",
    "speed": 5,
    "glide": 5,
    "turn": 0,
    "fade": 0,
    "plastic": "Star",
    "bagged": 1 (bool),
    "imageName": standard.png,
    "categoryId": 1,
    "brandId": 1,
  
}
```

## Category

| Metod  | Ändpunkt  | Beskrivning                                                                            |
| ------ | --------- | -------------------------------------------------------------------------------------- |
| GET    | /api/category | Hämtar alla tillgängliga kategorier.                                                       |
| GET    | /api/category/{id} | Hämtar en specifik kategori med angivet ID.                                                |
| POST   | /api/category | Lagrar en ny kategori. Kräver att ett kategori-objekt skickas med.                             |
| PUT    | /api/category/{id} | Uppdaterar en existerande kategori med angivet ID. Kräver att ett kategori-objekt skickas med. |
| DELETE | /api/category/{id} | Raderar en kategori med angivet ID.                                                        |

Ett kategori-objekt returneras/skickas som JSON med följande struktur:

```
{
    "categoryId": 2,
    "categoryName": "Putter"
  
}
```

## Brand

| Metod  | Ändpunkt  | Beskrivning                                                                            |
| ------ | --------- | -------------------------------------------------------------------------------------- |
| GET    | /api/brand | Hämtar alla tillgängliga märken.                                                       |
| GET    | /api/brand/{id} | Hämtar en specifik märke med angivet ID.                                                |
| POST   | /api/brand | Lagrar en ny märke. Kräver att ett märke-objekt skickas med.                             |
| PUT    | /api/brand/{id} | Uppdaterar en existerande märke med angivet ID. Kräver att ett märke-objekt skickas med. |
| DELETE | /api/brand/{id} | Raderar en märke med angivet ID.                                                        |

Ett märke-objekt returneras/skickas som JSON med följande struktur:

```
{
    "brandId": 2,
    "brandName": "Innova"
  
}
```
