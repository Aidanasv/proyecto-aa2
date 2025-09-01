# Etapa de construcción
FROM node:18-alpine as builder

WORKDIR /app

# Copiar archivos de configuración
COPY frontend/package*.json ./

# Instalar dependencias y limpiar cache
RUN npm ci && npm cache clean --force

# Copiar el código fuente
COPY frontend/ .

# Construir la aplicación
RUN npm run build

# Etapa de producción
FROM nginx:alpine

# Copiar los archivos construidos
COPY --from=builder /app/dist /usr/share/nginx/html

# Copiar configuración personalizada de nginx (opcional)
# COPY frontend/nginx.conf /etc/nginx/nginx.conf

# Exponer puerto
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
