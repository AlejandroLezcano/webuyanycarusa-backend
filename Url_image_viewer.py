from flask import Flask, Response, request
import requests

app = Flask(__name__)

@app.get("/test-image")
def test_image():
    # Get the image URL from query parameter
    IMAGE_URL = request.args.get('url')
    
    if not IMAGE_URL:
        return {"error": "URL parameter is required"}, 400
    
    # Fetch the blob from Azure
    r = requests.get(IMAGE_URL)

    # If the blob exists and responds OK
    if r.status_code == 200:
        return Response(
            r.content,
            mimetype="image/jpeg",             # ensures it's rendered inline
            headers={"Content-Disposition": "inline"}
        )
    else:
        return {"error": "Could not fetch image", "status": r.status_code}, 500

if __name__ == "__main__":
    app.run(debug=True)
