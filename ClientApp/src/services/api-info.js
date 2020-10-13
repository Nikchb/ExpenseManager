export default function getBaseUrl() {
    if (process.env.NODE_ENV === "development") {
        return "http://localhost:5000/api"
    }
    return "http://serverapp:80/api"
}