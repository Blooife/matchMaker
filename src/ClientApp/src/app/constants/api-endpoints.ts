export const base = "https://localhost:5000";

export const identityEndpoints = {
  login: `${base}/auth/login`,
  register: `${base}/auth/register`
}

export const bookEndpoints = {
  books: `${base}/books`
}

export const reviewEndpoints = {
  likes(reviewId: string) {
    return `${base}/reviews/${reviewId}/likes`;
  },
  dislikes(reviewId: string) {
    return `${base}/reviews/${reviewId}/dislikes`;
  },
  reviews: `${base}/reviews`
}

