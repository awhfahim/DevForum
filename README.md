# StackOverflow-Clone

##About Project Architecture

- **Clean Architecture**: The application follows the principles of Clean Architecture, with separate projects for the Domain, Application, Infrastructure, and Web layers. This separation of concerns ensures that the business rules and logic are decoupled from the infrastructure and presentation details, making the codebase easier to maintain and extend.

# StackOverflow Web Application

This is a web application that mimics the functionality of StackOverflow. It's built using C#, JavaScript, and the .NET 7.0 framework.

## Features

- **User Registration and Authentication**: Users can register with their email and password. The application supports secure authentication and session management.

- **Question Asking and Answering**: Registered users can post questions and provide answers to existing questions. Each question and answer can be upvoted or downvoted by users.

- **User Authorization Policies**: The application implements authorization policies to control access to certain features. For example, only logged-in users can post questions or answers.

- **Comments**: Users can comment on both questions and answers, providing a platform for discussion and clarification.

- **Markdown Support**: The application includes a markdown editor for posting questions and answers. This allows users to format their posts, include code snippets, and so on.

- **Profile Management**: Users can upload profile pictures and view/edit their profile information.

- **Notifications**: Users receive notifications when someone replies to their question or comment.

- **Tags**: Questions can be tagged with relevant keywords, making it easier for users to find the information they're looking for.

- **User-specific Views**: Users can view a list of their own questions, making it easy to keep track of discussions they're involved in.
  
- **AWS Integration**: The application is integrated with AWS services for various features. For example, AWS S3 might be used for storing user profile pictures, and AWS SES might be used for sending email notifications.
 
- **Bootstrap**: The application uses Bootstrap for responsive design and prebuilt components, which helps in faster and easier web development.
  
-  Bootstrap (included in the project)
  
- **Mapster**: The application uses Mapster for object-to-object mapping. It helps to transform between different data types, which is particularly useful when working with data transfer objects (DTOs).
  
- **SMTP Email Services**: The application uses SMTP for sending emails.
  
- **Stored Procedures**: The application uses a stored procedure for question retrieval, which can provide performance benefits and security against SQL injection attacks.

- **Worker Service for Email Sending**: The application uses a worker service to handle email sending asynchronously, improving the responsiveness of the application.
- 
- **Unit Tests**: The application has unit tests to ensure the correctness of the code. We use NUnit as our testing framework.

## Prerequisites

- .NET 7.0 SDK
- Docker (optional)
- AWS CLI (optional)
- Google reCAPTCHA API keys
- Bootstrap (included in the project)
- Mapster (included in the project)
- SMTP Server Details
- SQL Server (for stored procedures)
- NUnit (for unit testing)

## AWS Configuration

If you're using AWS services, you'll need to configure your AWS credentials. You can do this by installing the AWS CLI and running `aws configure`, or by setting the `AWS:AccessKey` and `AWS:SecretKey` values in the `appsettings.json` file.

Please note that you should never commit your AWS credentials to the repository. If you're setting the credentials in `appsettings.json`, make sure that file is ignored in your `.gitignore`.

## Google reCAPTCHA Configuration

If you're using Google reCAPTCHA, you'll need to configure your reCAPTCHA API keys. You can do this by setting the `ReCaptcha:SiteKey` and `ReCaptcha:SecretKey` values in the `appsettings.json` file.

Please note that you should never commit your reCAPTCHA API keys to the repository. If you're setting the keys in `appsettings.json`, make sure that file is ignored in your `.gitignore`.

## Docker Configuration

The application is Dockerized and can be run using Docker. Here are the steps to build and run the application using Docker:

1. Build the Docker image
2. Run the Docker container
3. The application will be accessible at `http://localhost:8000`.

Please note that you might need to adjust the Docker commands according to your Dockerfile and application settings.

## SMTP Configuration

If you're using SMTP for email services, you'll need to configure your SMTP server details. You can do this by setting the `Smtp:Host`, `Smtp:Port`, `Smtp:Username`, and `Smtp:Password` values in the `appsettings.json` file.

Please note that you should never commit your SMTP server details to the repository. If you're setting the details in `appsettings.json`, make sure that file is ignored in your `.gitignore`.



## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Installation

1. Clone the repository: Use the command git clone <repository-url> to clone the repository to your local machine.  
2. Install .NET 7.0 SDK: You can download it from the official .NET download page.  
3. Install Docker (optional): If you plan to use Docker for running the application, you can download it from the official Docker download page.  
4. Install AWS CLI (optional): If you plan to use AWS services, you can download it from the official AWS CLI download page.  
5. Set up the prerequisites: You need to set up the prerequisites mentioned in the README file. This includes obtaining Google reCAPTCHA API keys, setting up an SMTP server for email services, setting up a SQL Server for stored procedures, and installing NUnit for unit testing.  
6. Restore the dependencies: Navigate to the project directory and run dotnet restore to restore the dependencies of the project.  
7. Build the project: Run dotnet build to build the project.  
8. Run the project: Run dotnet run to start the project.  
9. Build the Docker image: Run docker-compose build to build the Docker images for the services defined in the docker-compose.yml file.  
10. Run the Docker containers: Run docker-compose up to start the Docker containers.
