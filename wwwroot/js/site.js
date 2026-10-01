/* ============================================================
   EURASIA BUS - MAIN SITE
   ============================================================ */

html {
    font - size: 16px;
    min - height: 100 %;
}

body {
    margin: 0;
    min - height: 100vh;
    font - family: Arial, Helvetica, sans - serif;
    background: #f5f7fb;
    color: #1f2937;
}

/* ============================================================
   NAVBAR
   ============================================================ */

.navbar {
    min - height: 70px;
}

.navbar - brand {
    font - size: 1.25rem;
    letter - spacing: 0.3px;
}

.navbar.nav - link {
    font - weight: 500;
    padding - left: 0.85rem;
    padding - right: 0.85rem;
}

/* ============================================================
   HERO
   ============================================================ */

.eb - hero {
    position: relative;
    padding: 80px 0 125px 0;
    background: linear - gradient(135deg, #071d3a 0 %, #0d3b66 55 %, #145da0 100 %);
    color: #ffffff;
    overflow: hidden;
}

.eb - hero:after {
    content: "";
    position: absolute;
    width: 420px;
    height: 420px;
    right: -160px;
    top: -180px;
    border - radius: 50 %;
    background: rgba(255, 255, 255, 0.06);
}

.eb - hero - content {
    position: relative;
    z - index: 2;
    max - width: 850px;
    margin: 0 auto;
    text - align: center;
}

.eb - hero - logo {
    display: flex;
    justify - content: center;
    align - items: center;
    gap: 15px;
    margin - bottom: 28px;
}

.eb - hero - logo - icon {
    width: 62px;
    height: 62px;
    border - radius: 16px;
    display: flex;
    align - items: center;
    justify - content: center;
    background: rgba(255, 255, 255, 0.14);
    font - size: 30px;
}

.eb - hero - brand {
    font - size: 1.75rem;
    font - weight: 700;
    letter - spacing: 1px;
}

.eb - hero - brand strong {
    color: #61dafb;
}

.eb - hero - subtitle {
    margin - top: 3px;
    color: rgba(255, 255, 255, 0.75);
    font - size: 0.9rem;
}

.eb - hero - title {
    margin: 0 auto;
    max - width: 850px;
    font - size: 3.2rem;
    line - height: 1.15;
    font - weight: 800;
    letter - spacing: -1px;
}

.eb - hero - title span {
    display: block;
    color: #61dafb;
}

.eb - hero - description {
    max - width: 650px;
    margin: 24px auto 28px auto;
    font - size: 1.1rem;
    line - height: 1.7;
    color: rgba(255, 255, 255, 0.82);
}

/* ============================================================
   HERO AUTH BUTTONS
   ============================================================ */

.hero - auth - buttons {
    display: flex;
    justify - content: center;
    align - items: center;
    gap: 12px;
    margin - top: 25px;
}

.hero - auth - button {
    display: inline - flex;
    align - items: center;
    justify - content: center;
    gap: 8px;
    min - width: 145px;
    padding: 12px 22px;
    border - radius: 9px;
    text - decoration: none;
    font - weight: 600;
    border: 1px solid transparent;
    cursor: pointer;
    transition: 0.2s ease;
}

.hero - auth - login {
    color: #ffffff;
    background: #1769aa;
    border - color: #3c8dcc;
}

.hero - auth - login:hover {
    color: #ffffff;
    background: #2180c7;
}

.hero - auth - register {
    color: #0b3157;
    background: #ffffff;
}

.hero - auth - register:hover {
    color: #0b3157;
    background: #e9f6ff;
}

/* ============================================================
   SEARCH SECTION
   ============================================================ */

.eb - search - section {
    position: relative;
    margin - top: -70px;
    padding - bottom: 70px;
    z - index: 5;
}

.eb - search - card {
    background: #ffffff;
    border - radius: 18px;
    padding: 30px;
    box - shadow: 0 15px 45px rgba(15, 35, 65, 0.16);
    border: 1px solid #e5eaf0;
}

.eb - search - header {
    display: flex;
    align - items: center;
    gap: 15px;
    margin - bottom: 25px;
}

.eb - search - header - icon {
    width: 52px;
    height: 52px;
    flex - shrink: 0;
    border - radius: 13px;
    display: flex;
    align - items: center;
    justify - content: center;
    background: #e8f4ff;
    color: #1261a0;
    font - size: 22px;
}

.eb - search - header h2 {
    margin: 0;
    font - size: 1.55rem;
    font - weight: 700;
    color: #172b4d;
}

.eb - search - header p {
    margin: 4px 0 0 0;
    color: #6b7280;
}

.eb - search - form.form - label {
    display: block;
    margin - bottom: 8px;
    font - weight: 600;
    color: #26364d;
}

.eb - search - form.form - label i {
    color: #1261a0;
    margin - right: 5px;
}

.eb - search - input {
    min - height: 48px;
    border: 1px solid #d6dee8;
    border - radius: 9px;
    background: #ffffff;
}

.eb - search - input:focus {
    border - color: #2477b9;
    box - shadow: 0 0 0 3px rgba(36, 119, 185, 0.12);
}

.eb - route - swap {
    width: 46px;
    height: 46px;
    border: 1px solid #cdd8e4;
    border - radius: 50 %;
    background: #ffffff;
    color: #1261a0;
    cursor: pointer;
    font - size: 18px;
    transition: 0.2s ease;
}

.eb - route - swap:hover {
    background: #1261a0;
    color: #ffffff;
    border - color: #1261a0;
}

.eb - search - button {
    min - height: 48px;
    border: none;
    border - radius: 9px;
    background: #1261a0;
    color: #ffffff;
    font - weight: 700;
    padding: 10px 18px;
}

.eb - search - button:hover {
    background: #0d4e83;
    color: #ffffff;
}

/* ============================================================
   FEATURES
   ============================================================ */

.eb - features - section {
    padding: 80px 0;
    background: #ffffff;
}

.eb - section - heading {
    max - width: 700px;
    margin: 0 auto 45px auto;
    text - align: center;
}

.eb - section - kicker {
    display: inline - block;
    margin - bottom: 10px;
    color: #1261a0;
    font - size: 0.8rem;
    font - weight: 800;
    letter - spacing: 2px;
}

.eb - section - heading h2 {
    margin: 0 0 12px 0;
    color: #172b4d;
    font - size: 2.25rem;
    font - weight: 800;
}

.eb - section - heading p {
    margin: 0;
    color: #687386;
    line - height: 1.7;
}

.eb - feature - card {
    height: 100 %;
    padding: 30px 24px;
    border: 1px solid #e4eaf1;
    border - radius: 16px;
    background: #ffffff;
    text - align: center;
    box - shadow: 0 8px 25px rgba(20, 45, 75, 0.06);
}

.eb - feature - icon {
    width: 62px;
    height: 62px;
    margin: 0 auto 20px auto;
    border - radius: 16px;
    display: flex;
    align - items: center;
    justify - content: center;
    background: #eaf5ff;
    color: #1261a0;
    font - size: 26px;
}

.eb - feature - card h3 {
    margin: 0 0 12px 0;
    color: #172b4d;
    font - size: 1.2rem;
    font - weight: 700;
}

.eb - feature - card p {
    margin: 0;
    color: #6b7280;
    line - height: 1.65;
}

/* ============================================================
   EUROPE SECTION
   ============================================================ */

.eb - europe - section {
    padding: 90px 0;
    background: #f5f8fc;
}

.eb - europe - title {
    margin: 0 0 20px 0;
    color: #172b4d;
    font - size: 2.7rem;
    line - height: 1.15;
    font - weight: 800;
}

.eb - europe - title span {
    display: block;
    color: #1261a0;
}

.eb - europe - description {
    max - width: 580px;
    margin - bottom: 28px;
    color: #687386;
    line - height: 1.8;
    font - size: 1.05rem;
}

.eb - europe - stats {
    display: flex;
    gap: 38px;
    margin: 30px 0;
}

.eb - stat - item {
    display: flex;
    flex - direction: column;
}

.eb - stat - item strong {
    color: #1261a0;
    font - size: 1.8rem;
    font - weight: 800;
}

.eb - stat - item span {
    color: #687386;
    font - size: 0.9rem;
}

.eb - primary - button {
    padding: 12px 22px;
    border - radius: 9px;
    background: #1261a0;
    color: #ffffff;
    font - weight: 700;
}

.eb - primary - button:hover {
    background: #0d4e83;
    color: #ffffff;
}

.eb - europe - visual {
    min - height: 390px;
    position: relative;
    display: flex;
    align - items: center;
    justify - content: center;
    overflow: hidden;
    border - radius: 25px;
    background: linear - gradient(145deg, #0b3157, #1261a0);
}

.eb - europe - circle {
    position: absolute;
    border - radius: 50 %;
    border: 1px solid rgba(255, 255, 255, 0.14);
}

.eb - europe - circle - 1 {
    width: 310px;
    height: 310px;
}

.eb - europe - circle - 2 {
    width: 220px;
    height: 220px;
}

.eb - bus - visual {
    position: relative;
    z - index: 2;
    text - align: center;
    color: #ffffff;
}

.eb - bus - visual - icon {
    font - size: 75px;
    margin - bottom: 10px;
}

.eb - bus - visual - title {
    font - size: 1.8rem;
    font - weight: 800;
    letter - spacing: 1px;
}

.eb - bus - visual - subtitle {
    margin - top: 5px;
    color: rgba(255, 255, 255, 0.72);
    font - size: 0.8rem;
    letter - spacing: 2px;
}

/* ============================================================
   CTA
   ============================================================ */

.eb - cta - section {
    padding: 70px 0;
    background: #ffffff;
}

.eb - cta - card {
    display: flex;
    align - items: center;
    gap: 25px;
    padding: 35px;
    border - radius: 18px;
    background: linear - gradient(135deg, #0b3157, #1261a0);
    color: #ffffff;
}

.eb - cta - icon {
    width: 65px;
    height: 65px;
    flex - shrink: 0;
    border - radius: 16px;
    display: flex;
    align - items: center;
    justify - content: center;
    background: rgba(255, 255, 255, 0.13);
    font - size: 28px;
}

.eb - cta - content {
    flex: 1;
}

.eb - cta - content h2 {
    margin: 0 0 8px 0;
    font - size: 1.6rem;
    font - weight: 800;
}

.eb - cta - content p {
    margin: 0;
    color: rgba(255, 255, 255, 0.76);
}

.eb - cta - button {
    white - space: nowrap;
    padding: 12px 22px;
    border - radius: 9px;
    background: #ffffff;
    color: #0b3157;
    font - weight: 700;
}

.eb - cta - button:hover {
    background: #eaf5ff;
    color: #0b3157;
}

/* ============================================================
   FOOTER
   ============================================================ */

.footer {
    background: #071d3a;
    color: rgba(255, 255, 255, 0.75);
}

.footer h5,
.footer h6 {
    color: #ffffff;
}

.footer a {
    color: rgba(255, 255, 255, 0.75);
}

.footer a:hover {
    color: #ffffff;
}

/* ============================================================
   VALIDATION
   ============================================================ */

.field - validation - error,
.validation - summary - errors {
    font - size: 0.85rem;
}

/* ============================================================
   MOBILE
   ============================================================ */

@media(max - width: 991px) {
    .eb - hero {
        padding: 65px 0 110px 0;
    }

    .eb - hero - title {
        font - size: 2.6rem;
    }

    .eb - search - section {
        margin - top: -55px;
    }

    .eb - route - swap {
        margin - top: 4px;
    }

    .eb - europe - title {
        font - size: 2.3rem;
    }
}

@media(max - width: 767px) {
    .eb - hero {
        padding: 50px 0 90px 0;
    }

    .eb - hero - logo {
        flex - direction: column;
    }

    .eb - hero - title {
        font - size: 2.1rem;
    }

    .eb - hero - description {
        font - size: 1rem;
    }

    .hero - auth - buttons {
        flex - direction: column;
    }

    .hero - auth - button {
        width: 100 %;
        max - width: 260px;
    }

    .eb - search - card {
        padding: 20px;
    }

    .eb - search - header {
        align - items: flex - start;
    }

    .eb - section - heading h2 {
        font - size: 1.9rem;
    }

    .eb - europe - stats {
        gap: 20px;
        flex - wrap: wrap;
    }

    .eb - europe - visual {
        min - height: 300px;
    }

    .eb - cta - card {
        flex - direction: column;
        text - align: center;
    }

    .eb - cta - action {
        width: 100 %;
    }

    .eb - cta - button {
        display: block;
        width: 100 %;
    }
}