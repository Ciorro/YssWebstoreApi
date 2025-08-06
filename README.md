# YssWebstoreAPI

YssWebstoreAPI is the backend for the YssStore - a social media platform for sharing games, tools and assets. Written in C#, using ASP.NET, LiteBus, Dapper and PostgreSQL.

## Endpoints

Test the API: [YssWebstoreAPI (Swagger)](https://api.store.yss.ct8.pl/swagger)

### Base Search Parameters

- `q` (string): General search query.
- `SortOptions.OrderBy` (string): Field to order results by.
- `SortOptions.Order` (string, enum: DESC, ASC): Sorting order.
- `PageOptions.Page` (integer): Page number.
- `PageOptions.PageSize` (integer): Number of results per page.

### Accounts

#### GET /api/Accounts

Retrieves a list of accounts.

- Query Parameters:
  - Base Search Parameters
  - `FollowedBy` (string, uuid): Filter accounts followed by a specific user.
  - `Following` (string, uuid): Filter accounts following a specific user.

#### GET /api/Accounts/:uniqueName

Retrieves an account by its unique name.

- Path Parameters:
  - `uniqueName` (string, required): The unique name of the account.

#### GET /api/Accounts/me

Retrieves the currently authenticated user's account information.

#### POST /api/Accounts/avatar

Uploads an avatar image for the current user.

- Request Body:
  - `file` (binary): The avatar image file.

#### DELETE /api/Accounts/avatar

Deletes the avatar for the current user.

#### POST /api/Accounts/:accountId/follows

Follows an account.

- Path Parameters:
  - `accountId` (string, uuid, required): The ID of the account to follow.

#### DELETE /api/Accounts/:accountId/follows

Unfollows an account.

- Path Parameters:
  - `accountId` (string, uuid, required): The ID of the account to unfollow.

#### POST /api/Accounts/verify

Verifies an account using a token.

- Request Body:
  - `token` (string): The verification token.

#### POST /api/Accounts/generate-verification-code

Generates a new verification code for the current account.

## Auth

#### POST /api/Auth/signup

Registers a new user account.

- Request Body:
  - `email` (string, email, nullable): User's email.
  - `password` (string, nullable): User's password.
  - `uniqueName` (string, nullable): Unique username.
  - `displayName` (string, nullable): User's display name.

#### POST /api/Auth/signin

Authenticates a user and generates a session token.

- Request Body:
  - `email` (string, email, nullable): User's email.
  - `password` (string, nullable): User's password.
  - `deviceInfo` (string, nullable): Device information.

#### POST /api/Auth/signout

Signs out the current user session.

- Request Body:
  - `sessionToken` (string): The session token to sign out.

#### POST /api/Auth/signout-all

Signs out all active sessions for the current user.

#### POST /api/Auth/refresh

Refreshes an authentication token.

- Request Body:
  - `accountId` (string, uuid): The account ID.
  - `sessionToken` (string, nullable): The session token to refresh.

## Posts

#### GET /api/Posts

Retrieves a list of posts.

- Query Parameters:
  - Base Search Parameters
  - `Account` (string): Filter posts by account.
  - `Project` (string, uuid): Filter posts related to a specific project.

#### POST /api/Posts

Creates a new post.

- Request Body:
  - `title` (string, nullable): Title of the post.
  - `content` (string, nullable): Content of the post.
  - `targetProjectId` (string, uuid, nullable): ID of the target project.

#### GET /api/Posts/:postId

Retrieves a single post by its ID.

- Path Parameters:
  - `postId` (string, uuid, required): The ID of the post.

#### POST /api/Posts/:postId/image

Uploads an image for a specific post.

- Path Parameters:
  - `postId` (string, uuid, required): The ID of the post.
- Request Body:
  - `file` (binary): The image file.

#### DELETE /api/Posts/:postId/image

Deletes an image from a specific post.

- Path Parameters:
  - `postId` (string, uuid, required): The ID of the post.

## Projects

#### GET /api/Projects

Retrieves a list of projects.

- Query Parameters:
  - Base Search Parameters
  - `Account` (string): Filter projects by account.
  - `Tags` (array of strings): Filter projects by tags.
  - `PinnedOnly` (boolean): Show only pinned projects.

#### POST /api/Projects

Creates a new project.

- Request Body:
  - `name` (string, nullable): Name of the project.
  - `description` (string, nullable): Description of the project.
  - `tags` (array of strings, nullable): Tags associated with the project.

#### GET /api/Projects/:slug

Retrieves a project by its unique slug.

- Path Parameters:
  - `slug` (string, required): The slug of the project.

#### POST /api/Projects/:projectId/icon

Uploads an icon for a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
- Request Body:
  - `file` (binary): The icon file.

#### DELETE /api/Projects/:projectId/icon

Deletes a project's icon.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.

#### POST /api/Projects/:projectId/images

Uploads an image to a project's gallery.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
- Request Body:
  - `file` (binary): The image file.

#### DELETE /api/Projects/:projectId/images/:imageId

Deletes an image from a project's gallery.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
  - `imageId` (string, uuid, required): The ID of the image.

#### PUT /api/Projects/:projectId/images/:imageId/order

Updates the order of an image in a project's gallery.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
  - `imageId` (string, uuid, required): The ID of the image.
- Request Body:
  - `order` (integer, int): The new order of the image.

#### GET /api/Projects/:projectId/packages

Retrieves a list of packages for a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.

#### POST /api/Projects/:projectId/packages

Uploads a new package for a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
- Query Parameters:
  - `Name` (string, required): Name of the package.
  - `Version` (string, required): Version of the package.
  - `TargetOS` (string, enum: Any, Windows, Linux, Mac, Android): Target operating system.
- Request Body:
  - `File` (binary, required): The package file.

#### DELETE /api/Projects/:projectId/packages/:packageId

Deletes a package from a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
  - `packageId` (string, uuid, required): The ID of the package.

#### PUT /api/Projects/:projectId/packages/:packageId

Updates a package's information.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
  - `packageId` (string, uuid, required): The ID of the package.
- Request Body:
  - `name` (string, nullable): New name for the package.

#### GET /api/Projects/:projectId/packages/:packageId/download

Downloads a specific package.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
  - `packageId` (string, uuid, required): The ID of the package.

#### POST /api/Projects/:projectId/pin

Pins a project to the current user's profile.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.

#### DELETE /api/Projects/:projectId/pin

Unpins a project from the current user's profile.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.

## Reviews

#### GET /api/projects/:projectId/Reviews

Retrieves a list of reviews for a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
- Query Parameters:
  - Base Search Parameters
  - `AccountId` (string, uuid): Filter reviews by a specific account.

#### POST /api/projects/:projectId/Reviews

Creates a new review for a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
- Request Body:
  - `rate` (integer, int): The rating for the review.
  - `content` (string, nullable): The content of the review.

#### PUT /api/projects/:projectId/Reviews

Updates an existing review for a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
- Request Body:
  - `rate` (integer, int): The updated rating.
  - `content` (string, nullable): The updated content.

#### DELETE /api/projects/:projectId/Reviews

Deletes a review for a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.

#### GET /api/projects/:projectId/Reviews/summary

Retrieves a summary of reviews for a project.

- Path Parameters:
  - `projectId` (string, uuid, required): The ID of the project.
