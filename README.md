# Habit Tracker

A fullstack web application for tracking habits and managing daily, weekly or monthly windows with "cheat days". Built with React, .NET 5, and SQLite.

## Overview

This application helps users maintain habits by:
- Setting up trackable habits with customizable time windows
- Managing allowable "cheat days" within each window
- Visualizing progress and remaining cheat days
- Securing data with user authentication

## Tech Stack

### Frontend
- React 17 with TypeScript
- Semantic UI React components
- Axios for API communication
- React Router v6
- Context API for state management
- Docker/Nginx deployment

### Backend
- .NET 5 Web API
- Entity Framework Core with SQLite
- MediatR (CQRS pattern)
- AutoMapper
- JWT authentication
- OpenAPI/Swagger docs

## Getting Started

### Prerequisites
- Node.js 16+
- .NET 5 SDK
- Docker (optional)

### Local Development

1. Clone the repo:
```bash
git clone https://github.com/yourusername/habit-tracker.git
cd habit-tracker
```

2. Start the backend:
```bash
cd backend/API
dotnet run
```

3. Start the frontend:
```bash 
cd client
npm install
npm start
```

Access the application:
- Frontend: http://localhost:3000
- API: http://localhost:5075
- API docs: http://localhost:5075/swagger

### Available Scripts

#### Frontend
- `npm start` - Run development server
- `npm test` - Run test suite
- `npm run build` - Create production build

#### Backend
- `dotnet run` - Start the API
- `dotnet test` - Run unit tests
- `dotnet watch run` - Hot-reload development

## Project Structure

```
habit-tracker/
├── client/              # React frontend
│   ├── src/
│   ├── public/
│   └── package.json
└── backend/             # .NET API
    ├── API/
    ├── Application/
    ├── Domain/
    └── Infrastructure/
```

## License

[MIT](https://choosealicense.com/licenses/mit/)