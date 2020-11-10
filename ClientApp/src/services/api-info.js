export default function getBaseUrl() {
    if (process.env.NODE_ENV === "Production") {
        return ""
    }
    return "http://localhost:5000/api"    
}