var express = require('express');
var router = express.Router();
const { GoogleSpreadsheet } = require('google-spreadsheet');

router.get('/', async function(req, res, next) {
    console.log('GET');
    res.sendStatus(200);
})

router.put('/', async function(req, res, next) {
    console.log('Got body:', req.body);
    surveyData = req.body.surveyData;
    parameters = req.body.parameters;
    systemInfo = req.body.systemInfo;

    const creds = require('../config/seniordesign-295702-20b0b5b1f562.json'); 
    const doc = new GoogleSpreadsheet('182pH2ziYRDjZgKGFqklPuNw87SB9Yna18S4ypn1UjYU');
    await doc.useServiceAccountAuth(creds);
    await doc.loadInfo();

    timestamp = new Date().toUTCString();
    const sheet = doc.sheetsByIndex[0];
    const larryRow = await sheet.addRow({
         "Question 1": surveyData.Question_0[0],
         "Question 2": surveyData.Question_1[0],
         "Question 3": surveyData.Question_2[0],
         "Question 4": surveyData.Question_3[0],
         "Lap ID": parameters.lapNumber,
         "FPS": parameters.fps,
         "Resolution Scale": parameters.resolutionMultiple,
         "Latency": parameters.q_len,
         "FPS Variance": parameters.fps_var,
         "Device Type": systemInfo.deviceType,
         "Device Model": systemInfo.deviceModel,
         "Device UID": systemInfo.deviceUniqueIdentifier,
         "Operating System": systemInfo.operatingSystem,
         "Processor": systemInfo.processorType,
         "Survey Code": parameters.surveyCode,
         "Timestamp (UTC)": timestamp
        });

    res.sendStatus(200);
})

module.exports = router;