# 🚀 OpenSAIL — Semantic API Intent Layer

[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![Build](https://img.shields.io/badge/build-passing-brightgreen)](#)
[![NuGet](https://img.shields.io/nuget/v/OpenSAIL.Core.svg)](https://www.nuget.org/packages/OpenSAIL.Core)
[![OpenAPI](https://img.shields.io/badge/openapi-3.0-blue.svg)](#)
[![Built with AI](https://img.shields.io/badge/built%20with-ChatGPT-ff69b4?logo=openai&labelColor=262626)](https://openai.com/chatgpt)

> **OpenSAIL** (Semantic API Intent Layer) is a lightweight and extensible protocol to describe the **intent** of your REST endpoints using natural language — enabling seamless integration with Large Language Models (LLMs), smart frontends, and CLI agents without cluttering your controller code.

---

## ✨ Why OpenSAIL?

- 🔌 Zero-intrusion: no attributes or controller annotations
- 🧠 `x-ai-*` metadata injection directly into your Swagger/OpenAPI output
- 📄 Managed via a standalone `sail.manifest.json` file
- 🤖 LLM-aware: perfect for prompt-routing, Copilot-style tools, and AI agents
- 🛠️ Fully compatible with existing REST APIs and Swagger tooling

---

## 📦 Installation

### ✅ Via NuGet

```bash
dotnet add package OpenSAIL.Core
```

---

## ⚙️ How to Use

### 1. Add the package to your ASP.NET Web API project

```bash
dotnet add package OpenSAIL.Core
```

### 2. Create a `sail.manifest.json` file in your project root

```json
{
  "endpoints": [
    {
      "path": "/weatherforecast",
      "method": "get",
      "intent": "get_weatherforecast",
      "description": "List all weather predictions for a specific day.",
      "meaning": "The response is a JSON array with date, summary, and temperature in Celsius.",
      "examples": [
        "Is today going to be cold?",
        "What is the weather prediction for tomorrow?"
      ]
    }
  ]
}
```

### 3. Register it in `Program.cs`

```csharp
builder.Services.AddSwaggerGen();
builder.Services.AddAiExtensions("sail.manifest.json");

var app = builder.Build();

app.UseSwagger(); // Only needed if you want to expose the enriched openapi.json
app.MapControllers();
app.Run();
```

---

## 📄 Manifest Structure (`sail.manifest.json`)

```json
{
  "endpoints": [
    {
      "path": "/route",
      "method": "get|post|put|delete",
      "intent": "action_identifier",
      "description": "Human-readable explanation of what the endpoint does.",
      "meaning": "What the response represents (useful for AI interpretation).",
      "examples": [
        "Natural phrases a user might say"
      ]
    }
  ]
}
```

---

## 🧠 Result

The `openapi.json` will include injected semantic metadata like:

```json
"x-ai-intent": "get_weatherforecast",
"x-ai-description": "List all weather predictions for a specific day.",
"x-ai-meaning": "The response is a JSON array with date, summary, and temperature in Celsius.",
"x-ai-examples": [
  "Is today going to be cold?",
  "What is the weather prediction for tomorrow?"
]
```

This enriched spec can be consumed by:

- LLM Agents (LangChain, AutoGen, Guidance)
- Smart frontends (`useApi()` hooks, prompt suggesters)
- CLI wrappers or prompt-based workflows

---

## 🔐 License

[MIT](LICENSE)

---

## 🛣️ Roadmap

- [x] OpenAPI 3.0+ support
- [x] Manifest-based metadata injection
- [x] Semantic enrichment for Swagger
- [ ] JSON Schema validation for manifest
- [ ] CLI: generate manifest from OpenAPI
- [ ] Integration example with LLM agent or prompt-router

---

> Built with ❤️ and precision by [Pedro Arruda](https://github.com/pedroarrudant)  
> Envisioned for the next generation of API–AI interoperability.