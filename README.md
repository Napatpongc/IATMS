# IATMS (Backend)

> **Backend API** ระบบติดตามการเข้างานของนักศึกษาฝึกงาน  

---

## Table of Contents

1. [Project Overview](#1-project-overview)
2. [Tech Stack](#2-tech-stack)
3. [System Requirements](#3-system-requirements)
4. [Installation & Setup](#4-installation--setup)
5. [Configuration](#5-configuration)
6. [Project Structure](#6-project-structure)
7. [Authentication Flow](#7-authentication-flow)
8. [Database](#8-database)
9. [Environment Guide](#9-environment-guide)
10. [Contact & Handover](#11-contact--handover)


---

## 1. Project Overview

**IATMS (Intern Attendance Tracking Management System)** คือระบบ Backend API สำหรับจัดการและติดตามการเข้างานของนักศึกษาฝึกงานภายในองค์กร

> โปรเจกต์นี้เป็นส่วน **Backend API** เท่านั้น — Frontend อยู่ใน repository แยก

---

## 2. Tech Stack

| ส่วน | เทคโนโลยี | Version |
|------|-----------|---------|
| Framework | ASP.NET Core | .NET 10 |
| ภาษา | C# | 13+ |
| Database | Microsoft SQL Server | 2019+ |
| DB Libary | ADO.NET (`Microsoft.Data.SqlClient`) | 6.1.4 |
| Authentication | JWT Bearer + LDAP (Active Directory) | — |



---

## 3. System Requirements

| รายการ | ความต้องการ |
|--------|------------|
| .NET SDK | 10.0 หรือสูงกว่า |
| SQL Server | 2019 หรือสูงกว่า |
| LDAP Server | Active Directory ที่เข้าถึงได้จาก network |
| OS | Windows / Linux / macOS (ที่รองรับ .NET 10) |


ตรวจสอบ .NET version:
```bash
dotnet --version
# ต้องแสดง 10.x.x
```

---

## 4. Installation & Setup

### 4.1 Clone Repository

```bash
git clone https://github.com/Napatpongc/IATMS.git
cd IATMS
```

### 4.2 Restore Dependencies

```bash
dotnet restore
```

### 4.3 ตั้งค่า Configuration

คัดลอกและแก้ไขไฟล์ configuration (ดูรายละเอียดใน [Section 5](#5-configuration)):

```bash
# แก้ไขค่าใน appsettings.json 
```

### 4.4 รันโปรเจกต์

```bash
dotnet run
```

API จะเปิดที่:
- `https://localhost:7xxx` (HTTPS)
- `http://localhost:5xxx` (HTTP)

---

## 5. Configuration

แก้ไขไฟล์ `appsettings.json` ก่อนใช้งาน:

```json
{
  "ConnectionStrings": {
    "LocalCon": "data source=YOUR_SERVER,1433;initial catalog=IATS_db;user id=YOUR_USER;password=YOUR_PASSWORD;...",
    "UATConn":  "data source=YOUR_UAT_SERVER,1433;initial catalog=IATS_db;user id=YOUR_USER;password=YOUR_PASSWORD;..."
  },
  "Env": {
    "db": "UATConn"
  },
  "Jwt": {
    "AccessSecret":  "YOUR_ACCESS_SECRET_KEY_MIN_32_CHARS",
    "RefreshSecret": "YOUR_REFRESH_SECRET_KEY_MIN_32_CHARS",
    "AccessLiftTime":  30,
    "RefreshLiftTime": 60
  },
  "Ldap": {
    "Server": "YOUR_LDAP_SERVER_IP",
    "Port":   "389"
  },
  "LdapSearchUser":     "YOUR_LDAP_SEARCH_USER",
  "LdapSearchPassword": "YOUR_LDAP_SEARCH_PASSWORD",
  "LdapName":           "DC=yourdomain,DC=local",
  "AllowedHosts": "http://your-frontend-url"
}
```

### คำอธิบาย Configuration

| Key | คำอธิบาย |
|-----|---------|
| `ConnectionStrings.LocalCon` | Connection string สำหรับ local development |
| `ConnectionStrings.UATConn` | Connection string สำหรับ UAT environment |
| `Env.db` | เลือก environment ที่ใช้งาน (`"LocalCon"` หรือ `"UATConn"`) |
| `Jwt.AccessSecret` | Secret key สำหรับเซ็น Access Token (ต้องยาวอย่างน้อย 32 ตัวอักษร) |
| `Jwt.RefreshSecret` | Secret key สำหรับเซ็น Refresh Token |
| `Jwt.AccessLiftTime` | อายุ Access Token (นาที) — ค่าเริ่มต้น 30 นาที |
| `Jwt.RefreshLiftTime` | อายุ Refresh Token (นาที) — ค่าเริ่มต้น 60 นาที |
| `Ldap.Server` | IP Address ของ LDAP / Active Directory server |
| `Ldap.Port` | Port ของ LDAP — ค่าเริ่มต้น 389 |
| `LdapSearchUser` | Username สำหรับค้นหาใน LDAP directory |
| `LdapSearchPassword` | Password ของ LDAP search user |
| `LdapName` | Base DN ของ Active Directory domain |
| `AllowedHosts` | URL ของ Frontend ที่อนุญาตให้เรียก API (CORS) |

> ⚠️ **อย่า commit ค่าจริงของ Secret, Password ลงใน Git** — ใช้ Environment Variables หรือ Secret Manager แทน

---

## 6. Project Structure

```
IATMS/
│
├── Authorization/              # Custom authorization logic
│   └── ...                    # Handler, policy สำหรับ JWT
│
├── Components/                 # Shared components / utilities
│   └── ...                    # Helper functions ที่ใช้ร่วมกัน
│
├── Configurations/             # ตัวแปร configuration และ mapping
│   └── ...                    # Strongly-typed config classes
│
├── ConTextDB/                  # Database context
│   └── ...                    # SQL connection, query helpers
│
├── Controllers/                # API Endpoints (HTTP layer)  
│   └── ...
│
├── Models/                     # Data models / DTOs
│   ├── Request/               # Request payload models
│   └── Response/              # Response payload models
│
├── Properties/
│   └── launchSettings.json    # Local dev server settings
│
├── Program.cs                  # Entry point — middleware pipeline
├── appsettings.json            # Configuration (อย่าใส่ secret จริง)

```

### คำอธิบายแต่ละ Layer

| Folder | หน้าที่ |
|--------|--------|
| `Controllers/` | รับ HTTP request, validate input, เรียก logic, ส่ง response |
| `Models/` | กำหนดรูปแบบ JSON ที่รับ (Request) และส่งกลับ (Response) |
| `ConTextDB/` | จัดการ connection และ query กับ SQL Server |
| `Authorization/` | ตรวจสอบ JWT token และ permission |
| `Configurations/` | Map ค่าจาก `appsettings.json` เป็น C# class |
| `Components/` | Helper / utility ที่ใช้ร่วมกันหลาย controller |

---

## 7. Authentication Flow

```
Frontend                 IATMS API              LDAP Server          SQL Server
   │                        │                       │                     │
   │──POST /auth/login──────▶│                       │                     │
   │  {username, password}   │──ค้นหา user──────────▶│                     │
   │                         │◀─ user found ─────────│                     │
   │                         │──ตรวจสอบ password────▶│                     │
   │                         │◀─ auth success ───────│                     │
   │                         │──ดึงข้อมูล user────────────────────────────▶│
   │                         │◀─ user data ──────────────────────────────│
   │◀─ {accessToken,         │                       │                     │
   │    refreshToken} ───────│                       │                     │
   │                         │                       │                     │
   │──GET /api/... ──────────▶│                       │                     │
   │  Authorization: Bearer  │──validate JWT ────────│                     │
   │                         │◀─ valid ──────────────│                     │
   │◀─ response data ────────│                       │                     │
```

**Token Lifecycle:**
- **Access Token** อายุ **30 นาที** — ใช้กับทุก API call
- **Refresh Token** อายุ **60 นาที** — ใช้ขอ Access Token ใหม่
- เมื่อ Access Token หมดอายุ → เรียก `POST /api/auth/refresh`
- เมื่อ Refresh Token หมดอายุ → ต้อง login ใหม่

---

## 8. Database

- **DBMS:** Microsoft SQL Server
- **Database Name:** `IATS_db`
- **Default Port:** `1433`

### Connection String Format

```
data source=SERVER_IP,1433;
initial catalog=IATS_db;
persist security info=True;
user id=DB_USER;
password=DB_PASSWORD;
MultipleActiveResultSets=True;
TrustServerCertificate=True;
```
---

## 9. Environment Guide

โปรเจกต์รองรับ 2 environments ที่ตั้งค่าผ่าน `appsettings.json`:

| Environment | Key ใน `Env.db` | Server | วัตถุประสงค์ |
|-------------|----------------|--------|------------|
| Local | `"LocalCon"` | `192.168.1.100` | Development / ทดสอบบนเครื่อง |
| UAT | `"UATConn"` | `192.168.1.101` | ทดสอบก่อน deploy จริง |

**วิธี switch environment:**

แก้ค่าใน `appsettings.json`:
```json
"Env": {
  "db": "LocalCon"
}
```

หรือใช้ Environment Variable เมื่อ deploy:
```bash
export Env__db=UATConn
dotnet run
```

---


## 11. Contact & Handover

| รายการ | รายละเอียด |
|--------|----------|
| ผู้พัฒนา | ณภัทรพงศ์ แช่มช้อย, ณัฏฐพล ไพรรื่นรมย์ |
| Repository | https://github.com/Napatpongc/IATMS |
| Version | 1.0.0 |
| วันส่งมอบ | 31/03/2026 |

### สิ่งที่ต้องเตรียมก่อนใช้งาน

1. ✅ แก้ไข credentials ทั้งหมดใน `appsettings.json`
2. ✅ ตรวจสอบ LDAP server เข้าถึงได้จาก deploy server
3. ✅ สร้าง database `IATS_db` และ run schema script
4. ✅ ตั้งค่า CORS ให้ตรงกับ URL ของ Frontend
5. ✅ ทดสอบ login ผ่าน Swagger UI ก่อน go-live

---

*README นี้อ้างอิงจาก source code version 1.0.0 — หากมีการอัปเดต API กรุณาอัปเดต documentation ด้วย*
