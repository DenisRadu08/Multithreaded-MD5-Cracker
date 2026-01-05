# Multithreaded MD5 Cracker

A high-performance brute-force tool written in C# that utilizes multi-threading to reverse engineer MD5 hashes.

This project demonstrates the application of **parallel computing principles** to cryptographic challenges, optimizing the search through large solution spaces ($N^k$) by distributing the workload across available CPU cores.

## 🚀 Key Features

* **Multi-threaded Architecture:** Uses C# `System.Threading` to spawn multiple worker threads, significantly reducing execution time compared to single-threaded approaches.
* **Optimized Backtracking:** Implements a recursive backtracking algorithm to generate candidate strings from a custom character set (lowercase, numbers, symbols).
* **Search Space Partitioning:** The algorithm logically divides the potential password combinations among threads to avoid redundant checks and race conditions.
* **MD5 Hashing:** Utilizes `System.Security.Cryptography` for hash computation.

## 🛠️ Technical Implementation

### The Challenge
Brute-forcing a password involves exploring a Cartesian product of the character set. For a password of length `L` and a character set of size `N`, the complexity is $O(N^L)$.
* **Character Set:** 46 characters (`a-z`, `0-9`, symbols).
* **Search Space:** Increases exponentially (e.g., 4 characters = $46^4$ ≈ 4.4 million combinations).

### The Solution: Parallelism
Instead of a single linear search, the tool uses **Static Partitioning**:
1.  The main process determines the logical core count of the host machine.
2.  The search space is split based on the starting character index.
3.  Each thread operates independently on its assigned subset, notifying the main controller upon finding a collision.

## 💻 Technologies Used

* **Language:** C# (.NET Core / .NET Framework)
* **Concurrency:** `System.Threading.Thread`, `Monitor` / `Lock` mechanisms.
* **Cryptography:** `System.Security.Cryptography.MD5`

## ⚠️ Disclaimer

This tool was developed for **educational purposes only**, to demonstrate:
1.  The implementation of parallel algorithms in C#.
2.  The vulnerability of weak hashing algorithms (like MD5) against brute-force attacks.
3.  The importance of password complexity (entropy).

**Do not use this tool on unauthorized systems or data.**

## 👤 Author

**Denis-Răzvan Radu** Computer Engineering Student at Politehnica University of Timișoara.