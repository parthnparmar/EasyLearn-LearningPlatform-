# Student-Teacher Doubt Chat Module

## Overview
The Doubt Chat Module enables real-time communication between students and instructors for course-related queries and clarifications.

## Features

### For Students
- **Ask Doubts**: Create new doubt threads for enrolled courses
- **Real-time Chat**: Send and receive messages with instructors
- **Track Status**: Monitor doubt status (Open/Resolved)
- **View History**: Access all previous doubts and conversations

### For Instructors
- **View All Doubts**: See doubts from all enrolled students
- **Respond to Queries**: Reply to student doubts in real-time
- **Mark as Resolved**: Close doubts once answered
- **Unread Notifications**: See count of new messages

## Database Schema

### DoubtChats Table
- `Id` (int, PK)
- `StudentId` (string, FK to AspNetUsers)
- `InstructorId` (string, FK to AspNetUsers, nullable)
- `CourseId` (int, FK to Courses)
- `Subject` (string)
- `Status` (string: "Open" or "Resolved")
- `CreatedAt` (datetime)
- `ResolvedAt` (datetime, nullable)

### DoubtMessages Table
- `Id` (int, PK)
- `DoubtChatId` (int, FK to DoubtChats)
- `SenderId` (string, FK to AspNetUsers)
- `Message` (string)
- `SentAt` (datetime)
- `IsRead` (bool)

## Routes

### Student Routes
- `GET /doubts` - View all doubts
- `GET /doubts/create` - Create new doubt form
- `POST /doubts/create` - Submit new doubt
- `GET /doubts/{id}` - View specific doubt chat
- `POST /doubts/{id}/send` - Send message

### Instructor Routes
- `GET /doubts` - View all student doubts
- `GET /doubts/{id}` - View specific doubt chat
- `POST /doubts/{id}/send` - Send message
- `POST /doubts/{id}/resolve` - Mark doubt as resolved

## Usage

### Student Workflow
1. Navigate to "Doubts" from the navigation menu
2. Click "Ask New Doubt"
3. Select course, enter subject and doubt description
4. Submit and wait for instructor response
5. Continue conversation until resolved

### Instructor Workflow
1. Navigate to "Student Doubts" from the navigation menu
2. View list of all doubts with unread indicators
3. Click on a doubt to view conversation
4. Reply to student messages
5. Mark as resolved when answered

## Technical Implementation

### Service Layer
- `IDoubtChatService` - Interface for doubt chat operations
- `DoubtChatService` - Implementation with EF Core

### Controller
- `DoubtChatController` - Handles all doubt chat requests
- Role-based routing (Student/Instructor views)

### Views
- `StudentIndex.cshtml` - Student doubts list
- `InstructorIndex.cshtml` - Instructor doubts list
- `Create.cshtml` - Create new doubt form
- `Chat.cshtml` - Chat interface

## Future Enhancements
- Real-time notifications using SignalR
- File attachments support
- Search and filter doubts
- Email notifications
- Doubt categories/tags
- Rating system for responses
