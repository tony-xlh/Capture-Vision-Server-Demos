# ASP.NET Document Scanner

An ASP.NET document scanner web app. [Dynamsoft Document Normalizer](https://www.dynamsoft.com/document-normalizer/docs/core/introduction/) is used for document boundaries detection and cropping.

Backend API:

* POST `/api/Document/detect`
* POST `/api/Document/crop`
* POST `/api/Document/detectAndCrop`
* GET `/api/Document/{ID}`
* GET `/api/Document/cropped/{ID}`

