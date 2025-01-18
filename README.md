# YssWebstoreAPI

YssWebstoreAPi is the backend API for YssStore - a social media platform for sharing games, tools and assets. Written in C#, using ASP.NET, Dapper and MediatR.

## Endpoints

Endpoints marked with :key: require authentication with bearer token.
### Accounts

**GET /api/accounts/{uniqueName}**: Get public account.
```jsonc
//RESPONSE
{
  "id": 3,
  "createdAt": "2024-09-11T20:03:49+02:00",
  "updatedAt": "2025-01-14T20:06:57+01:00",
  "uniqueName": "ciorro",
  "displayName": "CiorroDev",
  "status": "🚂🚃🚃🚃"
}
```

:key: **GET /api/accounts**: Get private account.
```jsonc
//RESPONSE
{
  "id": 3,
  "createdAt": "2024-09-11T20:03:49+02:00",
  "updatedAt": "2025-01-14T20:06:57+01:00",
  "email": "example@email.com",
  "uniqueName": "ciorro",
  "displayName": "CiorroDev",
  "status": "🚂🚃🚃🚃"
}
```

:key: **PUT /api/accounts/{uniqueName}**: Update an account
```jsonc
//REQUEST
{
  "uniquename": "yss",
  "displayname": "Yellow Squares Studio",
  "status": ""
}

//RESPONSE
accountId
```

:key: **DELETE /api/accounts/{uniqueName}**: Delete an account
```
//RESPONSE
accountId
```

:key: **POST /api/accounts/{uniqueName}/follow**: Follow an account
```
//RESPONSE
accountId
```

:key: **DELETE /api/accouts/{uniqueName}/follow**: Unfollow an account
```
//RESPONSE
accountId
```
### Auth
**POST /api/auth/signup**: Create an account.
```jsonc
//REQUEST
{
  "uniquename": "ciorro",
  "displayname": "CiorroDev",
  "credentials": {
    "email": "example@email.com",
    "password": "password"
  }
}

//RESPONSE
{
  "accessToken": "***",
  "refreshToken": "***"
}
```

**POST /api/auth/signin**: Sign in to an existing account.
```jsonc
//REQUEST
{
  "email": "example@email.com",
  "password": "password"
}

//RESPONSE
{
  "accessToken": "***",
  "refreshToken": "***"
}
```

**POST /api/auth/{accountId}/refresh**: Get new refresh token.
```jsonc
//REQUEST
"RefreshToken"

//RESPONSE
{
  "accessToken": "***",
  "refreshToken": "***"
}
```

:key: **POST /api/auth/generateVerificationCode**: Generate a new verification code.
```
No content
```

:key: **POST /api/auth/verify**: Verify an account with a code
```
//REQUEST
123456
```

### Products
**GET /api/products/{productId}**: Get single product.
```jsonc
{
  "id": 1,
  "createdAt": "2024-09-13T01:24:13+02:00",
  "updatedAt": "2024-11-28T00:06:35+01:00",
  "account": {
    "id": 3,
    "createdAt": "2024-09-11T20:03:49+02:00",
    "updatedAt": "2025-01-14T20:06:57+01:00",
    "uniqueName": "ciorro",
    "displayName": "CiorroDev",
    "status": "🚂🚃🚃🚃"
  },
  "name": "Connect Paint",
  "description": "A long description of the app.",
  "sourceUrl": null,
  "rating": 3,
  "images": [
    "/products/1/img-1.jpg",
    "/products/1/img-0.jpg",
    "/products/1/img-2.jpg"
  ],
  "tags": [
    {
      "group": "category",
      "value": "tool"
    },
    {
      "group": "tag",
      "value": "2D"
    },
    {
      "group": "tag",
      "value": "art"
    }
  ],
  "supportedOS": 3
}
```

**GET /api/products/search**: Search for products. Supports following params:
- **accountName**: owner's unique name.
- **searchQuery**: a text to search by.
- **orderBy**: name of the property to search by.
- **descending**: true/false.
- **page**: page number.
- **pageSize**: number of search results per page.
```jsonc
{
  "pageNumber": 0,
  "pageSize": 2,
  "itemCount": 3,
  "items": [
    //Products...
  ]
}
```

:key: **POST /api/products**: Creates a new product.
```
//REQUEST 
//TODO

//RESPONSE
productId
```

:key: **PUT /api/products/{productId}**: Updates a product.
```
//REQUEST 
//TODO

//RESPONSE
productId
```

:key: **DELETE /api/products/{productId}**: Deletes a product.
```
//RESPONSE
productId
```

:key: **POST /api/products/{productId}/pin**: Pins the product to the profile page.
```
//RESPONSE
productId
```

:key: **DELETE /api/products/{productId}/pin**: Unpins the product from the profile page.
```
//RESPONSE
productId
```

### Packages
**GET /api/products/{productId}/packages/{packageId}**: Get single product package.
```jsonc
//RESPONSE
{
  "id": 1,
  "createdAt": "2024-09-12T16:27:58+02:00",
  "updatedAt": "2024-10-18T16:30:41+02:00",
  "productId": 1,
  "fileSize": 7992674,
  "name": "Release",
  "version": "0.1",
  "downloadUrl": "http://example.com/game.zip",
  "targetOS": 1
}
```

**GET /api/products/{productId}/packages**: Get all product packages.
```jsonc
//RESPONSE
[
  //Packages...
]
```

:key: **POST /api/products{productId}/packages**: Creates a new package for a product.
```jsonc
//REQUEST
{
  "productId": 1,
  "name": "Mac Release",
  "version": "0.1.2",
  "downloadUrl": "http://example.com/update.zip",
  "targetOS": 3
}

//RESPONSE
packageId
```

:key: **PUT /api/products/{productId}/packages/{packageId}**: Updates a package.
```jsonc
//REQUEST
{
  "name": "Windows Release"
}

//RESPONSE
packageId
```

:key: **DELETE /api/products/{productId}/packages/{packageId}**: Deletes a package.
```
//RESPONSE
packageId
```

### Reviews
**GET /api/products/{productId}/reviews**: Get product reviews. Supports following params:
- **page**: page number.
- **pageSize**: number of search results per page.
```jsonc
//RESPONSE
{
  "pageNumber": 0,
  "pageSize": 2,
  "itemCount": 2,
  "items": [
    //Reviews...
  ]
}
```

**GET /api/products/{productId}/reviews/account/{accountId}**: Get product review submitted by a spcific user.
```jsonc
//RESPONSE
{
  "id": 12,
  "createdAt": "2024-11-18T17:33:14+01:00",
  "updatedAt": "2024-12-02T18:43:23+01:00",
  "account": {
    "id": 3,
    "createdAt": "2024-09-11T20:03:49+02:00",
    "updatedAt": "2025-01-14T20:06:57+01:00",
    "uniqueName": "ciorro",
    "displayName": "CiorroDev",
    "status": "🚂🚃🚃🚃"
  },
  "rate": 5,
  "content": "STRONG"
}
```

**GET /api/products/{productId}/reviews/summary**: Get reviews summary for a product.
```jsonc
//RESPONSE
{
  "minRate": 1,
  "maxRate": 5,
  "average": 4.5,
  "totalCount": 2,
  "rates": [
    {
      "rate": 1,
      "count": 0,
      "share": 0
    },
    {
      "rate": 2,
      "count": 0,
      "share": 0
    },
    {
      "rate": 3,
      "count": 0,
      "share": 0
    },
    {
      "rate": 4,
      "count": 1,
      "share": 0.5
    },
    {
      "rate": 5,
      "count": 1,
      "share": 0.5
    }
  ]
}
```

:key: **POST /api/product/{productId}/reviews**: Creates a review for a product.
```jsonc
//REQUEST
{
	"rate": 2,
	"content": "🫳🏻🫳🏻🫳🏻"
}

//RESPONSE
reviewId
```

:key: **PUT /api/products/{productId}/reviews**: Updates a review.
```jsonc
//REQUEST
{
	"rate": 5,
	"content": "Super!"
}

//RESPONSE
reviewId
```

:key: **DELETE /api/products/{productId}/reviews**: Deletes a review.
```
//RESPONSE
reviewId
```
