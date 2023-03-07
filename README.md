# ZemogaTest

This application provides two endpoints for making API calls, `Posts` and `Comments`.

## Posts Endpoint

The Posts endpoint allows you to view, create, and delete posts.

### API Endpoints

| Endpoint | Method | Description |
| --- | --- | --- |
| `/posts` | GET | List all posts |
| `/posts/{postId}` | GET | Get a specific post |
| `/posts` | POST | Create a new post |
| `/posts/{postId}` | DELETE | Delete a specific post |

## Comments Endpoint

The Comments endpoint allows you to view and create comments.

### API Endpoints

| Endpoint | Method | Description |
| --- | --- | --- |
| `/comments` | GET | List all comments |
| `/comments/{commentId}` | GET | Get a specific comment |
| `/comments` | POST | Create a new comment |
