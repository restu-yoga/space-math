# SpaceMath Shooter

SpaceMath Shooter adalah game edukasi matematika berbasis Unity dengan tema luar angkasa. Game ini menggabungkan konsep permainan shooter 2D dengan latihan soal matematika dasar. Pemain harus menembak musuh yang memiliki angka jawaban sesuai dengan soal yang muncul di layar.

Game ini ditujukan untuk membantu pembelajaran matematika dasar agar lebih menarik, interaktif, dan menyenangkan.

## Deskripsi Project

Pada game ini, pemain akan mengendalikan pesawat luar angkasa. Soal matematika akan muncul di layar, kemudian beberapa musuh dengan angka berbeda akan muncul sebagai pilihan jawaban. Pemain harus menembak musuh yang memiliki angka sesuai dengan jawaban yang benar.

Operasi matematika yang digunakan meliputi:

- Penjumlahan
- Pengurangan
- Perkalian
- Pembagian

Range angka dibuat sederhana agar sesuai untuk pembelajaran matematika dasar.

## Fitur Utama

- Game 2D bertema luar angkasa
- Sistem soal matematika otomatis
- Operasi penjumlahan, pengurangan, perkalian, dan pembagian
- Soal perkalian berdasarkan tabel 1 sampai 10
- Soal pembagian dengan hasil bilangan bulat
- Sistem skor
- Sistem nyawa pemain
- Timer permainan
- Musuh sebagai pilihan jawaban
- Tampilan UI pertanyaan
- Tampilan Game Over
- Build dapat dijalankan di Windows

## Teknologi yang Digunakan

- Unity 2021.3.45f2
- C#
- TextMeshPro
- Git
- GitHub

## Struktur Folder Project

```text
SpaceMath/
├── Assets/
│   ├── Scripts/
│   ├── Scenes/
│   ├── Sprites/
│   ├── Prefabs/
│   ├── Audio/
│   └── UI/
├── Packages/
├── ProjectSettings/
├── .gitignore
└── README.md
```

## Script Utama

Beberapa script utama yang digunakan dalam project ini:

| Script | Fungsi |
|---|---|
| GameManager | Mengatur alur permainan, mulai game, game over, dan kondisi permainan |
| PlayerControl | Mengatur pergerakan pemain |
| EnemySpawner | Mengatur kemunculan musuh |
| QuestionGenerator | Membuat soal matematika secara otomatis |
| QuestionUIController | Menampilkan soal ke UI |
| TimeCounter | Mengatur timer permainan |

## Range Soal Matematika

| Operasi | Range Angka | Keterangan |
|---|---|---|
| Penjumlahan | 1 - 10 | Hasil maksimal 20 |
| Pengurangan | 1 - 10 | Hasil tidak negatif |
| Perkalian | 1 - 10 | Berdasarkan tabel perkalian dasar |
| Pembagian | 1 - 10 | Hasil selalu bilangan bulat |

## Contoh Soal

```text
3 + 5 = ?
9 - 4 = ?
6 × 7 = ?
40 ÷ 5 = ?
```

## Cara Bermain

1. Jalankan game.
2. Tekan tombol Play untuk memulai permainan.
3. Perhatikan soal matematika yang muncul di layar.
4. Cari musuh dengan angka jawaban yang benar.
5. Tembak musuh tersebut.
6. Jika jawaban benar, skor akan bertambah.
7. Jika salah atau terkena musuh, nyawa pemain dapat berkurang.
8. Permainan berakhir ketika nyawa habis.

## Cara Menjalankan Project di Unity

1. Clone repository ini:

```bash
git clone https://github.com/restu-yoga/space-math.git
```

2. Buka Unity Hub.

3. Klik tombol Add atau Open.

4. Pilih folder project yang sudah di-clone.

5. Gunakan Unity versi:

```text
Unity 2021.3.45f2
```

6. Buka scene utama di folder:

```text
Assets/Scenes/
```

7. Klik tombol Play di Unity Editor.

## Cara Build Game ke Windows

1. Buka Unity.
2. Pilih menu:

```text
File > Build Settings
```

3. Pilih platform:

```text
PC, Mac & Linux Standalone
```

4. Klik Switch Platform jika diperlukan.
5. Klik Build.
6. Pilih folder tujuan build.
7. Setelah build selesai, jalankan file `.exe`.

## Catatan Build Unity

Hasil build Unity Windows biasanya tidak hanya berupa satu file `.exe`. File dan folder seperti berikut tetap dibutuhkan agar game dapat berjalan:

```text
SpaceMath.exe
SpaceMath_Data/
UnityPlayer.dll
MonoBleedingEdge/
UnityCrashHandler64.exe
```

Jika ingin membagikan game, sebaiknya semua file hasil build dimasukkan ke dalam satu file `.zip`.

## Folder yang Tidak Diupload ke GitHub

Project ini menggunakan `.gitignore` agar folder hasil generate Unity tidak ikut masuk ke repository.

Folder yang tidak perlu diupload:

```text
Library/
Temp/
Obj/
Build/
Builds/
Logs/
UserSettings/
```

Folder tersebut tidak perlu dimasukkan ke GitHub karena dapat dibuat ulang secara otomatis oleh Unity.

## Cara Push Project ke GitHub

Jalankan command berikut di Git Bash dari folder project:

```bash
git init
git add .
git commit -m "Initial commit Unity project"
git branch -M main
git remote add origin https://github.com/restu-yoga/space-math.git
git push -u origin main
```

## Developer

Muh Restu Yoga Pratama

## Tujuan Project

Project ini dibuat sebagai media pembelajaran matematika dasar berbasis game. Dengan konsep shooter luar angkasa, pemain dapat belajar berhitung sambil bermain sehingga proses belajar menjadi lebih menarik.