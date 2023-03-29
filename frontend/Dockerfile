# Build container
FROM node:14-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm i
COPY . .
RUN npm run build --prod

# Final container
FROM nginx:alpine AS final
COPY ./nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist/booking-app /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]