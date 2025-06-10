JWT Token Refresh Flow (Angular + Secure Backend)
 Overview
This document outlines a secure and scalable JWT authentication strategy using access tokens stored in
memory and refresh tokens stored in HttpOnly cookies. This is ideal for real-world production Angular
applications.
 Step-by-Step Flow
 Step 1: User Login
Frontend: Sends a POST request to /api/login with user credentials.
Backend:
Verifies credentials
Returns:
Access token in JSON
Refresh token in HttpOnly , Secure , SameSite=Strict cookie
Example Response: 
Set-Cookie: refreshToken=abc123; HttpOnly; Secure; SameSite=Strict
 Step 2: Store Access Token in Memory
Angular stores the access token in a service (not in localStorage/sessionStorage). 
this.accessToken = response.accessToken;
 Step 3: Authenticated Requests
Angular sends API requests with the access token in the Authorization header: 
Authorization: Bearer <accessToken>
 Step 4: Access Token Expires
After expiry, backend returns: 
• 
• 
• 
• 
◦ 
◦ 
• 
• 
• 
1
HTTP 401 Unauthorized
 Step 5: Interceptor Catches 401
Angular interceptor intercepts the response and triggers /api/refresh-token .
 Step 6: Send Refresh Token
Browser automatically sends refresh token via cookie with: 
GET /api/refresh-token
 Step 7: Backend Validates Refresh Token
If valid, returns a new access token.
 Step 8: Update Token and Retry
Angular stores new access token and retries the failed request. 
this.accessToken = newAccessToken;
 Step 9: User Logout
Angular sends a POST /logout
Backend clears refresh token cookie 
Set-Cookie: refreshToken=; HttpOnly; Max-Age=0
Frontend clears access token from memory
 Security Summary
Token Type Storage Lifetime Accessible to JS Auto-sent in Requests
Access Token In Memory Short (15m) Yes (in code) No
Refresh Token HttpOnly Cookie Long (7d) No Yes (Cookie)
📦 Bonus Recommendations
Use token rotation for refresh tokens.
• 
• 
• 
• 
• 
• 
• 
• 
2
Protect all endpoints against CSRF/XSS.
Enable CORS with credentials: 
app.use(cors({ origin: 'http://localhost:4200', credentials: true }));
HTTPS is required for Secure cookies to work properly.