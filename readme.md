## AI-assisted Blog Website

This project documentation outlines the development of an AI-assisted blog website API. The platform enables users to log in and create blog posts with the assistance of AI. Users have the option to evaluate their blogs based on predetermined criteria, improve grammar, suggest new titles, or enhance the writing style with AI suggestions. The blogs are saved as markdown files, accessible through a specified markdown path.

**Major Features:**

- **Authentication**: We created an authentication and authorization system from scratch using jwt,
  this system provides a simple and easy way to protect routes in the application using action filters,
  the authentication is provided by the user service which itself uses JWTService that relies on HashService. to protect controller routes, simply you can put the [ServiceFilter(typeof(AuthMiddleware))] decorator on top of the controller method, this check if a bearer token exists in the request header and recuperates the user related to it, putting the user object in the request object
  so that it can be recuperated in the controller method. otherwise, it prevents access to the method and returns a 401 response.

- **File Upload Service**: We implemented a service for uploading markdown files and images, leveraging Microsoft Azure Blob Storage. This service includes methods for file validation (via the **fileValidationService**), uploading, and deleting markdown and image files. The **fileController** provides endpoints for file deletion and image uploading, returning URLs for direct use in the frontend. The **blogController** utilizes the **fileService** for uploading markdown files during blog creation.
- **AI Service and Controller**: The AI functionalities are managed by a dedicated service and controller, employing prompt engineering to elicit optimal responses from the GPT-3.5 model, using an OpenAI key. AI responses are formatted in JSON directly from the prompt parameters, tailored to the query and requirements (examples available in Swagger documentation).
- **CI/CD Pipeline**: Deployment is streamlined through a CI/CD pipeline using PM2, targeting a Digital Ocean VM and employing Nginx as a reverse proxy. This setup hosts both the frontend and backend components of the blog app, with automated redeployment via GitHub Actions upon every push to the master branch. The frontend is accessible at **https://blog.skandertebourbi.tech/**, and the backend at **https://skandertebourbi.tech/blog/api**.
- **Database**: The blog's database is hosted on a Digital Ocean VM, ensuring seamless data management and access.
- **Environment Configuration**: Essential configurations, including the Azure Blob Storage service connection string and the OpenAI key, are stored in the **.env** file, securing sensitive information.
- **ExceptionFilters**: We created a global exception filter to catch exceptions of type or of parent type BadResponseException, this class contains a status code and a message, when such exception is caught, the application recuperates the status code and the message and create a response object with such properties and returns it.

**Backend Development:**

The backend is developed with .NET 6, comprising controllers for blogs, comments, files, AI, and user management, alongside corresponding services, models, and DTOs. It integrates several middleware for administrative functions, authentication, and blog owner-specific actions (e.g., editing or deleting a blog). Authentication is secured with JWT, and passwords are protected using a specialized hashing service. Additionally, a bad request exception filter enhances error message clarity.

**Frontend Development:**

The frontend, although not fully completed, is built with Next.js and TypeScript. We employed Storybook to develop a UI component library/design system, accessible locally. The project integrates ESLint, Prettier, and Jest to maintain code quality and consistency across the team. A GitHub Actions CI pipeline ensures that pull requests adhere to linting rules and pass unit tests before merging. Implemented features include login/signup functionality with JWT authentication and a navigation bar.

**Learning Outcomes:**

Dotenet was super interesting to learn. It has proven to be super powerful and easy to learn as the team is familiar with MVC architecture. This was a great opportunity for us to explore AI and Devops along with the different tools that can help mantain a healthy codebase.
