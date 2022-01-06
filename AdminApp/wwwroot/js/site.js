// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

async function loadTrainingData(parsed_data){
    console.log(parsed_data)
    const faceDescriptors = []

    for(const item of parsed_data.data){
        const descriptors = []
        for(let i=0;i<item.studentImagePaths.length;i++){
            const image = await faceapi.fetchImage(`${item.studentImagePaths[i]}`)
            const detection = await faceapi.detectSingleFace(image).withFaceLandmarks().withFaceDescriptor()
            descriptors.push(detection.descriptor)
        }
        faceDescriptors.push(new faceapi.LabeledFaceDescriptors(item.studentName, descriptors))
        Toastify({text: `Training xong data của ${item.studentName}!`}).showToast();
    }
    return faceDescriptors
}

let faceMatcher

async function init(parsed_data) {
    console.log("Abc")
    await faceapi.nets.ssdMobilenetv1.loadFromUri('js/models')
    // await Promise.all([
    //     faceapi.loadSsdMobilenetv1Model('/js/models'),
    //     faceapi.loadFaceRecognitionModel('/js/models'),
    //     faceapi.loadFaceLandmarkModel('/js/models'),
    // ])

    Toastify({text: "Tải xong model nhận diện!",}).showToast();

    const trainingData = await loadTrainingData(parsed_data)
    faceMatcher = new faceapi.FaceMatcher(trainingData, 0.6)
    console.log(faceMatcher)
}

