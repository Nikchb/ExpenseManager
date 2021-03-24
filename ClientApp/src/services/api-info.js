export default function getBaseUrl() {
    if (process.env.NODE_ENV === "Production") {
        return process.env.SERVER_URL + "/api"
    }
    return "http://localhost:5000/api"    
}