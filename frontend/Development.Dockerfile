FROM node:16-alpine AS build
WORKDIR /app

COPY . .
RUN npm install
RUN npm run build:development
# Serve Application using Nginx Server
FROM nginx:alpine
COPY --from=build /app/dist/booking-app/ /usr/share/nginx/html
EXPOSE 80