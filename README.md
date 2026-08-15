# AzilEdu — Sustav za upravljanje azilom za životinje

Blazor Server + ASP.NET Core Web API aplikacija za evidenciju životinja, volontera, donatora, zaposlenika i donacija u azilu. Uključuje JWT autentifikaciju, autorizaciju temeljenu na ulogama i integraciju s AI servisom.

---

## Pokretanje aplikacije

### Preduvjeti

- .NET 10 SDK
- Visual Studio 2022+ ili VS Code s C# ekstenzijom

### 1. Pokretanje API projekta

```bash
cd AzilEdu.Api
dotnet run
```

API se pokreće na:
- HTTPS: `https://localhost:7205`
- HTTP: `http://localhost:5195`
- Swagger UI: `https://localhost:7205/swagger`

Pri prvom pokretanju automatski se primjenjuju sve EF Core migracije i seedaju demo podaci (korisnici, životinje, volonteri, donatori, djelatnici).

### 2. Pokretanje App projekta

```bash
cd AzilEdu.App
dotnet run
```

App se pokreće na:
- HTTPS: `https://localhost:7298`
- HTTP: `http://localhost:5163`

**Važno:** API mora biti pokrenut prije App projekta jer App odmah komunicira s `https://localhost:7205`.

### Pokretanje iz Visual Studio

Otvori `AzilEdu.slnx`, desni klik na Solution → *Set Startup Projects* → odaberi *Multiple startup projects* i postavi API i App na *Start*. Pritisni F5.

### Reset baze podataka

```bash
cd AzilEdu.Api
dotnet ef database update 0   # briše sve tablice
dotnet ef database update     # primjenjuje sve migracije od početka
```

---

## Demo korisnici

> Ovo su isključivo lokalni razvojni podaci. Ne koristiti u produkciji.

| Email | Lozinka | Uloge |
|---|---|---|
| `admin@aziledu.local` | `Admin123!` | Admin, User |
| `employee@aziledu.local` | `Employee123!` | Employee, User |
| `volunteer@aziledu.local` | `Volunteer123!` | Volunteer, User |
| `donor@aziledu.local` | `Donor123!` | Donor, User |

Admin račun ima pristup svim modulima. Volonter i donator vide samo vlastite podatke putem `/mine` endpointa.

---

## Relacije korisničkih računa

### AppUser → AppRole (putem AppUserRole)

`AppUser` i `AppRole` su u relaciji mnogo-prema-mnogo posredovanoj tablicom `AppUserRoles` s kompozitnim primarnim ključem `(AppUserId, AppRoleId)`. Jedan korisnik može imati više uloga (npr. Admin + User). Uloge su ugrađene u JWT token kao `role` claimovi i provjeravaju se pri svakom zahtjevu pomoću `[Authorize(Roles = "...")]`.

### AppUser → Volunteer

`AppUser` ima opcionalni strani ključ `VolunteerId` koji pokazuje na tablicu `Volunteers`. Relacija je 1:1 — jedan korisnički račun može biti povezan s najviše jednim volonterom. Kada je korisnik prijavljen s ulogom Volunteer i ima postavljen `VolunteerId`, API endpointovi `/mine` čitaju taj ID iz JWT tokena (claim `volunteerId`) umjesto iz URL parametra, čime se sprječava pristup tuđim podacima.

### AppUser → Donor

Isti obrazac kao za Volunteer — opcionalni strani ključ `DonorId`. Donator može pristupiti samo donacijama koje su evidentirane pod njegovim `DonorId` putem endpointa `GET /api/donations/mine`.

### AppUser → Employee

Opcionalni strani ključ `EmployeeId` koji povezuje korisnički račun s profilom djelatnika. Djelatnici imaju ulogu `Employee` i pristup operativnim modulima (životinje, zadaci, donatori, donacije).

---

## Razlika između 401 i 403

| Status | Značenje | Kada se pojavljuje |
|---|---|---|
| **401 Unauthorized** | Identitet nije potvrđen | Zahtjev nema Bearer token, token je istekao ili je nevažeći. API ne zna tko si. |
| **403 Forbidden** | Pristup odbijen | Token je valjan i API zna tko si, ali nemaš dovoljno prava. Npr. volonter pokušava otvoriti `/api/users`. |

Praktično pravilo: 401 = nisi se prijavio, 403 = prijavljen si ali nije dozvoljeno.

---

## AI endpointi

Svi AI endpointi šalju podatke **samo prema konfiguriranom provideru** (Mock ili OpenAI). API nikad ne sprema AI odgovor samostalno — odgovor se vraća klijentu koji ga može urediti ili odbaciti prije eventualnog spremanja kroz standardni CUD tok.

| Endpoint | Metoda | Autorizacija | Što se šalje provideru |
|---|---|---|---|
| `GET /api/ai/status` | GET | Admin, Employee | Ništa — vraća naziv providera i modela |
| `POST /api/ai/text` | POST | Admin, Employee | Svrha (`animal-adoption`, `donor-thank-you`, `social-post`) + korisnički unos (max 4000 znakova) |
| `GET /api/ai/daily-summary` | GET | Admin, Employee | Agregirani brojevi iz baze: ukupno životinja, dostupnih, otvorenih zadataka, zakašnjelih, donacija u 7 dana |
| `GET /api/ai/volunteer-summary/mine` | GET | Volunteer | Popis do 10 otvorenih zadataka prijavljenog volontera (naslov, tip, životinja, status, rok) |
| `POST /api/ai/animal-intake` | POST | Admin, Employee | Slobodni tekst bilješke s terena (max 4000 znakova) |
| `POST /api/ai/animal-data-check` | POST | Admin, Employee | Podaci o životinji iz forme (ime, vrsta, pasmina, spol, dob, datum dolaska, status, opis) |

**Nikakvi osobni podaci (email, lozinka, adresa) nikad ne odlaze AI provideru.**

---

## Mock i OpenAI način rada

### Mock (zadano za razvoj)

`appsettings.json` je već konfiguriran za Mock:

```json
"Ai": {
  "Provider": "Mock",
  "Model": "gpt-5.6-luna",
  "ApiKey": ""
}
```

Mock servis vraća lokalne predvidljive odgovore bez mrežnog poziva.

### OpenAI (za produkcijski prikaz)

**Nikad ne stavljaj API ključ u `appsettings.json` ili u repozitorij.**

Koristi .NET User Secrets:

```bash
cd AzilEdu.Api
dotnet user-secrets set "Ai:Provider" "OpenAI"
dotnet user-secrets set "Ai:Model" "gpt-4o-mini"
dotnet user-secrets set "Ai:ApiKey" "sk-..."
```

User Secrets se čuvaju lokalno na `%APPDATA%\Microsoft\UserSecrets\<id>\secrets.json` i **nisu dio repozitorija** (`.gitignore` ih automatski isključuje).

Za prebacivanje natrag na Mock:

```bash
dotnet user-secrets set "Ai:Provider" "Mock"
```

---

## Zašto ključevi ostaju u API projektu

JWT `SigningKey` i AI `ApiKey` su konfigurirani isključivo u API projektu iz sigurnosnih razloga:

- **API** je jedini koji provjerava i potpisuje JWT tokene — App projekt nikad ne vidi ključ za potpisivanje
- **App** (Blazor Server) komunicira s API-jem kao klijent i prima gotov JWT — ne zna kako je potpisan
- AI pozivi idu isključivo s API strane — App projekt ne zna niti vidi AI API ključ
- Ako netko dobije pristup App kodu, ne može kompromitirati ni JWT ni AI integraciju

---

## Autorizacijski tok

```
UI akcija
  → DTO objekt
  → HTTP zahtjev s Authorization: Bearer <token>
  → JwtBearerMiddleware provjerava potpis i rok trajanja tokena (401 ako nevažeći)
  → [Authorize] atribut provjerava ulogu iz role claima (403 ako nedovoljno prava)
  → /mine endpointi čitaju volunteerId/donorId claim iz tokena (ne iz URL-a)
  → API kontroler
  → DbContext → SQLite baza
  → (opcionalno) AI servis → kontrolirani odgovor
  → JSON odgovor klijentu
  → Korisnik uređuje/odbacuje AI prijedlog
  → Eventualno spremanje kroz standardni CUD endpoint
```

---

## Poznata ograničenja

1. **Nema osvježavanja JWT tokena** — token traje 60 minuta, nakon isteka korisnik mora ponovo unijeti lozinku
2. **SQLite nije pogodan za produkciju** — nema konkurentnog pisanja, nema replikacije; za produkciju treba PostgreSQL ili SQL Server
3. **Medijske datoteke se čuvaju lokalno** (`wwwroot/uploads/animals/`) — ne rade u višeinstancijskom okruženju ni u oblaku bez dodatne konfiguracije
4. **AI ne pamti kontekst** — svaki poziv je neovisan, nema conversational history
5. **Volonteri i donatori ne mogu mijenjati vlastite podatke** — nema self-service profila

---

## Prijedlozi za sljedeću verziju

1. **Refresh token mehanizam** — dodati kratkotrajan access token (15 min) i dugotrajan refresh token koji se rotira pri svakom korištenju, čime se eliminira potreba za čestom ponovnom prijavom bez smanjivanja sigurnosti

2. **Pohrana medija u oblak** — zamijeniti lokalni filesystem s Azure Blob Storage ili AWS S3 uz generiranje SAS/presigned URL-ova, čime aplikacija postaje horizontalno skalabilna i mediji su dostupni neovisno o serveru

---

*Dokumentacija generirana za AzilEdu projekt — kolovoz 2026.*
