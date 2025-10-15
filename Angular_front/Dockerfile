FROM node:20-alpine AS build
WORKDIR /app

# Install dependencies
COPY package*.json ./
RUN npm ci

# Copy source and build (SSR configured in angular.json)
COPY . .
RUN npm run build

FROM node:20-alpine AS runtime
WORKDIR /app
ENV NODE_ENV=production
ENV PORT=4000

# Copy built artifacts only
COPY --from=build /app/dist/angular_front ./dist/angular_front

EXPOSE 4000
CMD ["node", "dist/angular_front/server/server.mjs"]

