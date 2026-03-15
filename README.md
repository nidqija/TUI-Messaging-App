# 📟 TUI Chat App with AI Integration

A high-performance, terminal-based chat application designed for developers. Featuring real-time messaging, multi-server scalability via Redis, and an integrated AI assistant.

---

## 🚀 1. Project Overview
This project is a **WhatsApp-inspired TUI (Terminal User Interface)** built for speed and developer productivity. It leverages WebSockets for real-time communication and a distributed architecture to handle concurrent users across multiple server instances.

* **Developer-First:** Full-featured chat experience without leaving the terminal.
* **AI-Powered:** Summon an AI assistant into any group for brainstorming or debugging.
* **Scalable:** Built with Redis Pub/Sub to synchronize messages across multiple backend nodes.

---

## 🛠 2. Tech Stack

| Component | Technology | Purpose |
| :--- | :--- | :--- |
| **Runtime** | [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) | .NET compiler for C# application |
| **Language** | C# | .NET ecosystem build |
| **TUI Framework** | (e.g., Blessed / Ink) | Terminal-based UI rendering |
| **Real-time** | WebSockets + Redis | Pub/Sub for cross-server messaging |
| **Database** | PostgreSQL | Persistent chat history and user data |
| **AI** | OpenAI / Ollama | LLM integration for `/ai` commands |
| **Environment** | WSL + Windows | Development workflow |

---

## 🏗 3. Core Architecture
The application uses a **Distributed WebSocket Architecture** to ensure horizontal scalability.

### Logical Flow:
1.  **Client:** The TUI connects to a Bun-based backend via WebSockets.
2.  **Ingress:** The server persists the message to **Postgres** for history.
3.  **Synchronization:** The server publishes the message to **Redis Pub/Sub**.
4.  **Broadcast:** All other server instances subscribed to the Redis channel receive the message and push it to their locally connected clients.

---

## ✨ 4. Features

### User Experience
* **Real-time Groups:** Join channels and chat with zero latency.
* **Persistent History:** View previous conversations upon joining.
* **AI Commands:**
    * `/ai join`: Bring the AI bot into the current room.
    * `/ai [query]`: Ask the bot a question.
    * `/ai leave`: Dismiss the AI bot.

### Technical Excellence
* **Async AI Processing:** AI responses are handled out-of-band to prevent WebSocket blocking.
* **State Management:** Decoupled storage (Postgres) and messaging (Redis).
* **Modular AI:** Easily swap between local (Ollama) and cloud (OpenAI) models.

---

## 📦 5. MVP Scope
For the initial release, the project focuses on:
* Supporting **5 concurrent users** in a single group.
* A single Bun server handling connections.
* Redis + Postgres running via **WSL**.
* Fully functional TUI client.

---

## 🗺 6. Roadmap

- [ ] **Phase 1: Environment Setup** (WSL, Redis, Postgres, Bun).
- [ ] **Phase 2: Core Chat Logic** (WebSocket implementation & Postgres persistence).
- [ ] **Phase 3: Scaling** (Redis Pub/Sub for multi-server support).
- [ ] **Phase 4: AI Integration** (Slash commands and async LLM responses).
- [ ] **Phase 5: Context Awareness** (RAG-style history for the AI).

---

## 📜 License
Distributed under the MIT License. See `LICENSE` for more information.
