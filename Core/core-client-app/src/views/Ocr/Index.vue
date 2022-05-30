<template>
    <div class="container ">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">
        </loading>
        <div class="row">
            <h2 id="header2">ยืนยันตัวตน</h2>
        </div>
        <div class="row mt-4">
            <div class="col-2" style="padding-right: 0px; width:13%;">
                <vs-checkbox v-model="acceptData" color="var(--main-color)">
                    <template #icon>
                        <i class='ti ti-check'></i>
                    </template>
                </vs-checkbox>
            </div>
            <div class="col-10 px-0">
                <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                    ข้าพเจ้าได้อ่าน รับทราบ และให้ความยินยอมใช้ข้อมูลส่วนบุคคล ในการขอใช้สิทธิ์เบิกค่าเสียหายเบื้องต้น/ค่าสินไหมทดแทน ตาม นโยบายคุ้มครองข้อมูลส่วนบุคคลของบริษัท
                </p>
            </div>
        </div>
        <div v-if="acceptData">
            <div class="box-container shadow-box px-2">
                <div class="row" align="center">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label title-advice-menu">อัพโหลดรูปภาพเพื่อยืนยันตัวตน</label>
                        </div>
                    </div>
                </div>
                <div class="row" v-if="os == 'android' || os == null">
                    <div class="col-6">
                        <img v-if="capIdCardDataUrl != null" :src="capIdCardDataUrl" width="100%" style="border-radius:5px" />
                        <button v-if="capIdCardDataUrl == null" class="btn-add-img-ocr" align="center" @click="showIdCardCam=!showIdCardCam">รูปบัตรประจำตัวประชาชน</button>
                        <button v-if="capIdCardDataUrl != null" class="btn-change-img-ocr" align="center" @click="removeIdCardFile">เปลี่ยนรูปภาพ</button>
                    </div>
                    <div class="col-6">
                        <b-overlay :show="isOverlay" rounded="sm">
                            <img v-if="capFaceDataUrl != null" :src="capFaceDataUrl" width="100%" style="border-radius:5px" />
                            <button v-if="capFaceDataUrl == null" class="btn-add-img-ocr" align="center" @click="showFaceCam=!showFaceCam">รูปถ่ายหน้าเซลฟี</button>
                            <button v-if="capFaceDataUrl != null" class="btn-change-img-ocr" align="center" @click="removeFaceFile">เปลี่ยนรูปภาพ</button>
                            <template #overlay>
                                <div class="text-center">
                                    <p id="cancel-label" style="font-size:10px">กรุณาอัพโหลดรูปบัตรประชาชนก่อน</p>
                                </div>
                            </template>
                        </b-overlay>
                    </div>
                </div>
                <div class="row" v-else-if="os != 'android'">
                    <div class="col-6">
                        <file-pond credits="null"
                                   label-idle="รูปบัตรประจำตัวประชาชน"
                                   :capture-method="null"
                                   accept="image/*"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   v-bind:files="idCardFile"
                                   v-on:addfile="onAddidCardFile"
                                   v-on:removefile="onRemoveIdCardFile"
                                   allowFileSizeValidation="true"
                                   maxFileSize="5MB"
                                   labelMaxFileSizeExceeded="รูปมีขนาดใหญ่เกินไป"
                                   labelMaxFileSize="ขนาดของรูปภาพต้องไม่เกิน {filesize}"
                                   ref="pondIdCard" />
                    </div>
                    <div class="col-6">
                        <b-overlay :show="isOverlay" rounded="sm">
                            <file-pond credits="null"
                                       label-idle="รูปถ่ายหน้าเซลฟี"
                                       v-bind:allow-multiple="false"
                                       v-bind:allowFileEncode="true"
                                       :capture-method="null"
                                       accept="image/*"
                                       v-bind:files="faceFile"
                                       v-on:addfile="onAddfaceFile"
                                       v-on:removefile="onRemoveFaceFile"
                                       allowFileSizeValidation="true"
                                       maxFileSize="5MB"
                                       labelMaxFileSizeExceeded="รูปมีขนาดใหญ่เกินไป"
                                       labelMaxFileSize="ขนาดของรูปภาพต้องไม่เกิน {filesize}"
                                       ref="pondFace" />
                            <template #overlay>
                                <div class="text-center">
                                    <p id="cancel-label" style="font-size:10px">กรุณาอัพโหลดรูปบัตรประชาชนก่อน</p>
                                </div>
                            </template>
                        </b-overlay>
                    </div>
                </div>


            </div>
            <b-overlay :show="isOverlay" rounded="sm">
                <div class="box-container  shadow-box px-2 mt-2 mb-5">
                    <div class="row mb-2" align="center">
                        <div class="col-12">
                            <div class="form-control-sm">
                                <label class="form-label title-advice-menu">ข้อมูลบัตรประจำตัวประชาชน</label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6" style="padding-right:5px">
                            <label class="form-label-ocr">เลขบัตรประจำตัว<span class="star-require">*</span></label>
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.idCardNo.$model" type="text" :class="{ 'is-invalid': $v.input.idCardNo.$error }" v-mask="'#-####-#####-##-#'"></b-form-input>
                            <div v-if="submitted && !$v.input.idCardNo.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเลขบัตรประชาชน</div>
                        </div>
                        <div class="col-6" style="padding-left:5px">
                            <label class="form-label-ocr">รหัสหลังบัตร<span class="star-require">*</span></label>
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.idcardLaserCode.$model" :class="{ 'is-invalid': $v.input.idcardLaserCode.$error }" v-mask="'AA#-#######-##'" :maxlength="20"></b-form-input>
                            <div v-if="submitted && !$v.input.idcardLaserCode.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกรหัสหลังบัตรประชาชน</div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-3" style="padding-right:0px">
                            <label class="form-label-ocr">คำนำหน้า<span class="star-require">*</span></label>
                            <v-select class="style-chooser mb-2" style="border:none;" v-model="$v.input.prefix.$model" :clearable="false" :options="prefixes" :class="{ 'is-invalid': $v.input.prefix.$error }"></v-select>
                            <div v-if="submitted && !$v.input.prefix.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกคำนำหน้า</div>
                        </div>
                        <div class="col-5 ">
                            <label class="form-label-ocr">ชื่อ<span class="star-require">*</span></label>
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.firstname.$model" :class="{ 'is-invalid': $v.input.firstname.$error }" :maxlength="50"></b-form-input>
                            <div v-if="submitted && !$v.input.firstname.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกชื่อ</div>
                        </div>
                        <div class="col-4 " style="padding-left:0px">
                            <label class="form-label-ocr">นามสกุล<span class="star-require">*</span></label>
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.lastname.$model" :class="{ 'is-invalid': $v.input.lastname.$error }" :maxlength="50"></b-form-input>
                            <div v-if="submitted && !$v.input.lastname.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกนามสกุล</div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6" style="padding-right:5px">
                            <label class="form-label-ocr">วันเกิด<span class="star-require">*</span></label>
                        </div>
                        <div class="col-6" style="padding-left:5px">
                            <label class="form-label-ocr">เบอร์โทร<span class="star-require">*</span></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6" style="padding-right:5px">
                            <v-date-picker v-model="$v.input.dateBirth.$model" class="flex-grow col-8" locale="th" mode="date" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                                <template v-slot="{ inputValue, inputEvents }">
                                    <input class=" mt-0 mb-2 form-control "
                                           :value="inputValue"
                                           v-on="inputEvents"
                                           :class="{ 'is-invalid': $v.input.dateBirth.$error }"
                                           readonly />
                                    <div v-if="submitted && !$v.input.dateBirth.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกวันเกิด</div>
                                </template>
                            </v-date-picker>
                        </div>
                        <div class="col-6" style="padding-left:5px">
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.telNo.$model" type="tel" :class="{ 'is-invalid': $v.input.telNo.$error }" v-mask="'###-###-####'"></b-form-input>
                            <div v-if="submitted && !$v.input.telNo.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเบอร์โทร</div>
                        </div>
                    </div>
                    <div class="row">
                        <span class="star-require">*หมายเหตุ กรุณาตรวจสอบความถูกต้อง/ครบถ้วน ของข้อมูลก่อนดำเนินการต่อ</span>
                    </div>
                </div>
                <template #overlay>
                    <div class="text-center">
                        <p id="cancel-label">กรุณาอัพโหลดรูปบัตรประจำตัวประชาชนก่อน</p>
                    </div>
                </template>
            </b-overlay>
            <div class="row">
                <div class="col-12">
                    <button class="btn-next-submit" style="width: 100%; padding: 8px 0px;" @click="submit">ดำเนินการต่อ</button>
                </div>
            </div>
            <br>
            <br>
        </div>
        <b-modal v-model="showIdCardCam"
                 ref="idCardModel"
                 hide-footer
                 hide-header
                 hide-header-close>
            <Rtc @base64="hiddenCamIdCard" @clickCanselBtn="onClickCanselBtn" :active="showIdCardCam" class="text-center"></Rtc>
        </b-modal>
        <b-modal v-model="showFaceCam"
                 ref="faceModel"
                 hide-footer
                 hide-header
                 hide-header-close>
            <Rtc @base64="hiddenCamFace" @clickCanselBtn="onClickCanselBtn" :active="showFaceCam" class="text-center"></Rtc>
        </b-modal>

    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'
    import liff from '@line/liff'
    import * as moment from "moment/moment";
    import axios from 'axios'
    import vueFilePond from 'vue-filepond';

    // Import FilePond styles
    import 'filepond/dist/filepond.min.css'

    // Import FilePond plugins
    // Please note that you need to install these plugins separately

    // Import image preview plugin styles
    import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css';

    // Import image preview and file type validation plugins
    import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type';
    import FilePondPluginImagePreview from 'filepond-plugin-image-preview';
    import FilePondPluginFileEncode from 'filepond-plugin-file-encode';
    import FilePondPluginFileValidateSize from 'filepond-plugin-file-validate-size';


    import Loading from 'vue-loading-overlay';

    import { required } from "vuelidate/lib/validators";

    // Create component
    const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview, FilePondPluginFileEncode, FilePondPluginFileValidateSize);

    import Rtc from '../../components/WebRTC/Rtc.vue'
    //Your Javascript lives within the Script Tag
    export default {
        name: "Ocr",
        mixins: [mixin],
        components: {
            Loading,
            FilePond,
            Rtc
        },
        data() {
            return {
                showIdCardCam: false,
                showFaceCam: false,
                capIdCardFile: null,
                capFaceFile: null,
                capIdCardDataUrl: null,
                capFaceDataUrl: null,
                os: null,
                attrs: [
                    {
                        key: 'today',
                        dot: true,
                        dates: new Date(),
                    },
                ],
                dateModelConfig: {
                    type: 'string',
                    mask: 'YYYY-MM-DD',
                },
                date: new Date(),
                idCardFile: null,
                faceFile: null,
                input: { idCardNo: null, idcardLaserCode: null, prefix: null, firstname: null, lastname: null, dateBirth: null, telNo: null, base64IdCard: null, base64Face: null, dataUrlIdCard: null, dataUrlFace: null },
                resultOCR: {},
                acceptData: false,
                prefixes: null,
                isLoading: true,
                submitted: false,
                isOverlay: true
            };
        },
        validations() {
            return {
                input: {
                    idCardNo: { required },
                    idcardLaserCode: { required },
                    prefix: { required },
                    firstname: { required },
                    lastname: { required },
                    dateBirth: { required },
                    telNo: { required },
                },
            }


        },

        methods: {
            onClickCanselBtn(action) {
                console.log(action)
                if (action === 'close-cam') {
                    this.showIdCardCam = false
                    this.showFaceCam = false
                }
            },
            removeIdCardFile() {
                this.capIdCardDataUrl = null
                this.isOverlay = true;
            },
            removeFaceFile() {
                this.capFaceDataUrl = null
            },
          
            hiddenCamIdCard(value) {
                if (value) {
                    this.isLoading = true;
                    this.showIdCardCam = false
                    this.capIdCard = this.dataURItoBlob(value)
                    var fileDataUrl = value
                    var imgRes = new Image();
                    imgRes.src = fileDataUrl
                    var img = document.createElement("img");
                    img.onload = () => {
                        var canvas = document.createElement("canvas");
                        var MAX_WIDTH = 720;
                        var MAX_HEIGHT = 720;
                        var width = img.width;
                        var height = img.height;

                        if (width > height) {
                            if (width > MAX_WIDTH) {
                                height *= MAX_WIDTH / width;
                                width = MAX_WIDTH;
                            }
                        } else {
                            if (height > MAX_HEIGHT) {
                                width *= MAX_HEIGHT / height;
                                height = MAX_HEIGHT;
                            }
                        }
                        canvas.width = width;
                        canvas.height = height;
                        var ctx = canvas.getContext("2d");
                        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                        this.capIdCardDataUrl = canvas.toDataURL("image/png");
                    }
                    img.src = fileDataUrl
                    //var url = process.env.VUE_APP_APPMAN_OCR_URL
                    //var bodyFormData = new FormData();
                    //bodyFormData.append('Username', process.env.VUE_APP_APPMAN_OCR_USERNAME);
                    //bodyFormData.append('Password', process.env.VUE_APP_APPMAN_OCR_PASSWORD);
                    //bodyFormData.append('image', this.capIdCard);

                    var url = this.$store.state.envUrl + '/api/User/Ocr'
                    var bodyFormData = new FormData();
                    bodyFormData.append('file', this.capIdCard);

                    axios.post(url, bodyFormData, {
                        headers: {
                            'Content-Type': 'multipart/form-data',
                            //'x-api-key': process.env.VUE_APP_APPMAN_X_API_KEY
                        }
                    }).then((response) => {
                        this.resultOCR = response.data.result;
                        if (response.status == 200) {
                            if (this.resultOCR.confidence.id_number < 0.9 || this.resultOCR.confidence.full_name_th < 0.9 || this.resultOCR.confidence.date_of_birth_th < 0.9) {
                                this.isLoading = false;
                                this.$swal({
                                    icon: 'error',
                                    text: 'รูปที่ท่านอัพโหลดไม่ชัด กรุณาลองใหม่',
                                    title: 'ผิดพลาด',
                                    showCancelButton: false,
                                    showDenyButton: false,
                                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                    confirmButtonColor: '#5c2e91',
                                    willClose: () => {
                                        this.removeIdCardFile()
                                    }
                                })
                            } else {                               
                                if (this.resultOCR != "") {
                                    this.input.idCardNo = this.resultOCR.id_number.replaceAll(" ", "-");
                                    this.input.prefix = this.resultOCR.title_th;
                                    this.input.firstname = this.resultOCR.first_name_th;
                                    this.input.lastname = this.resultOCR.last_name_th;
                                    this.input.dateBirth = moment(this.formatResultOcrDate(this.resultOCR.date_of_birth_en)).format("YYYY-MM-DD");
                                    this.input.base64IdCard = this.capIdCardDataUrl.replace(/^data:image\/(png|jpg);base64,/, "");

                                    this.isLoading = false;
                                    this.isOverlay = false;
                                }
                            }
                            
                            
                        } else if (response.status == 202) {
                            this.isLoading = false;
                            this.$swal({
                                icon: 'error',
                                text: 'รูปที่ท่านอัพโหลดไม่ใช่รูปบัตรประจำตัวประชาชน กรุณาลองใหม่',
                                title: 'ผิดพลาด',
                                showCancelButton: false,
                                showDenyButton: false,
                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {
                                    this.removeIdCardFile()
                                }
                            })
                        } else {
                            this.isLoading = false;
                            this.$swal({
                                icon: 'error',
                                text: 'อัพโหลดไม่สำเร็จกรุณาลองใหม่',
                                title: 'ผิดพลาด',
                                showCancelButton: false,
                                showDenyButton: false,
                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {
                                    this.removeIdCardFile()
                                }
                            })
                        }

                    }).catch(() => {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'error',
                            text: 'อัพโหลดไม่สำเร็จกรุณาลองใหม่',
                            title: 'ผิดพลาด',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                                this.removeIdCardFile()
                            }
                        })
                    });
                }

            },
            hiddenCamFace(value) {
                if (value) {
                    this.isLoading = true;
                    this.showFaceCam = false
                    this.capFaceDataUrl = value


                    var fileDataUrl = value
                    var imgRes = new Image();
                    imgRes.src = fileDataUrl
                    var img = document.createElement("img");
                    img.onload = () => {
                        var canvas = document.createElement("canvas");
                        var MAX_WIDTH = 720;
                        var MAX_HEIGHT = 720;
                        var width = img.width;
                        var height = img.height;

                        if (width > height) {
                            if (width > MAX_WIDTH) {
                                height *= MAX_WIDTH / width;
                                width = MAX_WIDTH;
                            }
                        } else {
                            if (height > MAX_HEIGHT) {
                                width *= MAX_HEIGHT / height;
                                height = MAX_HEIGHT;
                            }
                        }
                        canvas.width = width;
                        canvas.height = height;
                        var ctx = canvas.getContext("2d");
                        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                        this.capFaceDataUrl = canvas.toDataURL("image/png");
                        const body = {
                            faceBase64: this.capIdCardDataUrl.replace(/^data:image\/(png|jpg|jpeg);base64,/, ""),
                            idCardBase64: this.capFaceDataUrl.replace(/^data:image\/(png|jpg|jpeg);base64,/, "")

                        };
                        //const body = {
                        //    faceImage: this.capIdCardDataUrl.replace(/^data:image\/(png|jpg|jpeg);base64,/, ""),
                        //    identityImage: this.capFaceDataUrl.replace(/^data:image\/(png|jpg|jpeg);base64,/, "")

                        //};
                        var url = this.$store.state.envUrl + '/api/User/Ekyc'
                        //var url = process.env.VUE_APP_APPMAN_EKYC_URL
                        axios.post(url, body, {
                            headers: {
                                'Content-Type': 'application/json',
                                //'x-api-key': process.env.VUE_APP_APPMAN_X_API_KEY
                            }
                        }).then((response) => {
                            var result = response.data;
                            let resultCompare = (result.compare.result[0].similarity * 100).toFixed(2);
                            this.isLoading = false;
                            if (resultCompare < 70) {
                                this.$swal({
                                    icon: 'error',
                                    text: 'รูปถ่ายใบหน้าไม่ใช่คนเดียวกับบัตรประจำตัวประชาชนกรุณาลองใหม่อีกครั้ง',
                                    title: 'ผิดพลาด',
                                    /*footer: '<a href="">Why do I have this issue?</a>'*/
                                    showCancelButton: false,
                                    showDenyButton: false,

                                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                    confirmButtonColor: '#5c2e91',
                                    willClose: () => {
                                        this.removeFaceFile()
                                    }
                                })
                            }
                        }).catch(() => {
                            this.isLoading = false;
                            this.$swal({
                                icon: 'error',
                                text: 'เนื่องจากอัพโหลดไม่สำเร็จ หรือไม่ใช่รูปถ่ายใบหน้า กรุณาลองใหม่',
                                title: 'ผิดพลาด',
                                showCancelButton: false,
                                showDenyButton: false,
                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {
                                    this.removeFaceFile()
                                }
                            })
                        });

                    }
                    img.src = fileDataUrl

                }

            },
            dataURItoBlob(dataURI) {
                // convert base64 to raw binary data held in a string
                // doesn't handle URLEncoded DataURIs - see SO answer #6850276 for code that does this
                var byteString = atob(dataURI.split(',')[1]);

                // separate out the mime component
                var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

                // write the bytes of the string to an ArrayBuffer
                var ab = new ArrayBuffer(byteString.length);
                var ia = new Uint8Array(ab);
                for (var i = 0; i < byteString.length; i++) {
                    ia[i] = byteString.charCodeAt(i);
                }

                return new Blob([ab], { type: mimeString });


            },

            formatResultOcrDate(date_en) {
                date_en.replaceAll(" ", "-");
                if (date_en.includes("Jan.")) {
                    date_en.replace("Jan.", "01");
                } else if (date_en.includes("Feb.")) {
                    date_en.replace("Feb.", "02");
                } else if (date_en.includes("Mar.")) {
                    date_en.replace("Mar.", "03");
                } else if (date_en.includes("Apr.")) {
                    date_en.replace("Apr.", "04");
                } else if (date_en.includes("May.")) {
                    date_en.replace("May.", "05");
                } else if (date_en.includes("Jun.")) {
                    date_en.replace("Jun.", "06");
                } else if (date_en.includes("Jul.")) {
                    date_en.replace("Jul.", "07");
                } else if (date_en.includes("Aug.")) {
                    date_en.replace("Aug.", "08");
                } else if (date_en.includes("Sep.")) {
                    date_en.replace("Sep.", "09");
                } else if (date_en.includes("Oct.")) {
                    date_en.replace("Oct.", "10");
                } else if (date_en.includes("Nov.")) {
                    date_en.replace("Nov.", "11");
                } else if (date_en.includes("Dec.")) {
                    date_en.replace("Dec.", "12");
                }
                return date_en;
            },
            submit: async function () {
                if (this.os == 'android') {
                    if (this.capIdCardDataUrl == null) {
                        this.$swal({
                            icon: 'warning',
                            text: 'กรุณาอัพโหลดรูปบัตรประจำตัวประชาชน',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                        })
                        return false;
                    }
                    if (this.capFaceDataUrl == null) {
                        this.$swal({
                            icon: 'warning',
                            text: 'กรุณาอัพโหลดรูปถ่ายหน้าเซลฟี่',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                        })
                        return false;
                    }
                } else {
                    if (this.input.base64IdCard == null) {
                        this.$swal({
                            icon: 'warning',
                            text: 'กรุณาอัพโหลดรูปบัตรประจำตัวประชาชน',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                        })
                        return false;
                    }
                    if (this.input.base64Face == null) {
                        this.$swal({
                            icon: 'warning',
                            text: 'กรุณาอัพโหลดรูปถ่ายหน้าเซลฟี่',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                        })
                        return false;
                    }
                }

                this.submitted = true;

                // stop here if form is invalid
                this.$v.$touch();
                if (this.$v.$invalid) {
                    return false;
                }
                this.isLoading = true;
                await this.storeUserData()
            },
            storeUserData() {
                let body = null
                if (this.os == 'android') {
                    body = {
                        idcardNo: this.input.idCardNo,
                        idcardLaserCode: this.input.idcardLaserCode,
                        prefix: this.input.prefix,
                        fname: this.input.firstname,
                        lname: this.input.lastname,
                        stringDateofBirth: this.input.dateBirth,
                        mobileNo: this.input.telNo,
                        lineId: this.$store.state.userTokenLine,
                        base64IdCard: this.capIdCardDataUrl,
                        base64Face: this.capFaceDataUrl,
                    };
                } else {
                    body = {
                        idcardNo: this.input.idCardNo,
                        idcardLaserCode: this.input.idcardLaserCode,
                        prefix: this.input.prefix,
                        fname: this.input.firstname,
                        lname: this.input.lastname,
                        stringDateofBirth: this.input.dateBirth,
                        mobileNo: this.input.telNo,
                        lineId: this.$store.state.userTokenLine,
                        base64IdCard: this.input.dataUrlIdCard,
                        base64Face: this.input.dataUrlFace,
                    };
                }

                this.$swal({
                    icon: 'question',
                    text: 'ท่านยืนยันที่จะลงทะเบียนหรือไม่?',
                    /*title: 'แจ้งเตือน',*/
                    /*footer: '<a href="">Why do I have this issue?</a>'*/
                    showCancelButton: false,
                    showDenyButton: true,
                    denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยกเลิก",
                    denyButtonColor: '#dad5e9',
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยืนยัน",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.isLoading = false;
                    }
                }).then((result) => {
                    this.$store.state.inputUserData = body
                    if (result.isConfirmed) {
                        this.$router.push({ name: 'ConfirmOTP', params: { id: "Register", from: "CreateUser" } })
                    }
                    //} else if (result.isDenied) {

                    //}
                });
            },

            onRemoveIdCardFile: function () {
                this.input.base64IdCard = null
                this.input.dataUrlIdCard = null
            },
            onAddidCardFile: async function (error, file) {
                this.isLoading = true;
                this.input.base64IdCard = file.getFileEncodeBase64String()
                if (error != null) {
                    this.isLoading = false;
                    this.$swal({
                        icon: 'error',
                        text: error.sub,
                        title: error.main,
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {
                            this.$refs.pondIdCard.removeFiles()
                        }
                    })
                } else {
                    if (file) {
                        var fileDataUrl = file.getFileEncodeDataURL()
                        var imgRes = new Image();
                        imgRes.src = fileDataUrl
                        var img = document.createElement("img");
                        img.onload = () => {
                            var canvas = document.createElement("canvas");
                            var MAX_WIDTH = 720;
                            var MAX_HEIGHT = 720;
                            var width = img.width;
                            var height = img.height;

                            if (width > height) {
                                if (width > MAX_WIDTH) {
                                    height *= MAX_WIDTH / width;
                                    width = MAX_WIDTH;
                                }
                            } else {
                                if (height > MAX_HEIGHT) {
                                    width *= MAX_HEIGHT / height;
                                    height = MAX_HEIGHT;
                                }
                            }
                            canvas.width = width;
                            canvas.height = height;
                            var ctx = canvas.getContext("2d");
                            ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                            this.input.dataUrlIdCard = canvas.toDataURL(file.file.type);
                        }
                        img.src = fileDataUrl
                    }
                }



                //var url = process.env.VUE_APP_APPMAN_OCR_URL
                //var bodyFormData = new FormData();
                //bodyFormData.append('Username', process.env.VUE_APP_APPMAN_OCR_USERNAME);
                //bodyFormData.append('Password', process.env.VUE_APP_APPMAN_OCR_PASSWORD);
                //bodyFormData.append('image', file.file);

                var url = this.$store.state.envUrl + '/api/User/Ocr'
                var bodyFormData = new FormData();
                bodyFormData.append('file', file.file);

                axios.post(url, bodyFormData, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        //'x-api-key': process.env.VUE_APP_APPMAN_X_API_KEY
                    }
                }).then((response) => {
                    this.resultOCR = response.data.result;
                    if (response.status == 200) {
                        if (this.resultOCR.confidence.id_number < 0.9 || this.resultOCR.confidence.full_name_th < 0.9 || this.resultOCR.confidence.date_of_birth_th < 0.9) {
                            this.isLoading = false;
                            this.$swal({
                                icon: 'error',
                                text: 'รูปที่ท่านอัพโหลดไม่ชัด กรุณาลองใหม่',
                                title: 'ผิดพลาด',
                                showCancelButton: false,
                                showDenyButton: false,
                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {
                                    this.$refs.pondIdCard.removeFiles()
                                }
                            })
                        } else {
                            if (this.resultOCR != "") {
                                this.input.idCardNo = this.resultOCR.id_number.replaceAll(" ", "-");
                                this.input.prefix = this.resultOCR.title_th;
                                this.input.firstname = this.resultOCR.first_name_th;
                                this.input.lastname = this.resultOCR.last_name_th;
                                this.input.dateBirth = moment(this.formatResultOcrDate(this.resultOCR.date_of_birth_en)).format("YYYY-MM-DD");
                                this.input.base64IdCard = file.getFileEncodeBase64String();

                                this.isLoading = false;
                                this.isOverlay = false;
                            }
                        }
                        
                    } else if (response.status == 202) {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'error',
                            text: 'รูปที่ท่านอัพโหลดไม่ใช่รูปบัตรประจำตัวประชาชน กรุณาลองใหม่',
                            title: 'ผิดพลาด',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                                this.$refs.pondIdCard.removeFiles()
                            }
                        })
                    } else {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'error',
                            text: 'อัพโหลดไม่สำเร็จกรุณาลองใหม่',
                            title: 'ผิดพลาด',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                                this.$refs.pondIdCard.removeFiles()
                            }
                        })
                    }

                }).catch(() => {
                    this.isLoading = false;
                    this.$swal({
                        icon: 'error',
                        text: 'อัพโหลดไม่สำเร็จกรุณาลองใหม่',
                        title: 'ผิดพลาด',
                        showCancelButton: false,
                        showDenyButton: false,
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {
                            this.$refs.pondIdCard.removeFiles()
                        }
                    })
                });
            },
            onRemoveFaceFile: function () {
                this.input.base64Face = null
                this.input.dataUrlFace = null
            },
            onAddfaceFile: async function (error, file) {
                this.isLoading = true;
                this.input.base64Face = file.getFileEncodeBase64String()
                if (error != null) {
                    this.isLoading = false;
                    this.$swal({
                        icon: 'error',
                        text: error.sub,
                        title: error.main,
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {
                            this.$refs.pondFace.removeFiles()

                        }
                    })
                } else {
                    if (file) {
                        var fileDataUrl = file.getFileEncodeDataURL()
                        var imgRes = new Image();
                        imgRes.src = fileDataUrl
                        var img = document.createElement("img");
                        img.onload = () => {
                            var canvas = document.createElement("canvas");
                            var MAX_WIDTH = 720;
                            var MAX_HEIGHT = 720;
                            var width = img.width;
                            var height = img.height;

                            if (width > height) {
                                if (width > MAX_WIDTH) {
                                    height *= MAX_WIDTH / width;
                                    width = MAX_WIDTH;
                                }
                            } else {
                                if (height > MAX_HEIGHT) {
                                    width *= MAX_HEIGHT / height;
                                    height = MAX_HEIGHT;
                                }
                            }
                            canvas.width = width;
                            canvas.height = height;
                            var ctx = canvas.getContext("2d");
                            ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                            this.input.dataUrlFace = canvas.toDataURL(file.file.type);

                            const body = {
                                faceBase64: this.input.dataUrlFace.replace(/^data:image\/(png|jpg|jpeg);base64,/, ""),
                                idCardBase64: this.input.dataUrlIdCard.replace(/^data:image\/(png|jpg|jpeg);base64,/, "")

                            };
                            //const body = {
                            //    faceImage: this.input.dataUrlFace.replace(/^data:image\/(png|jpg|jpeg);base64,/, ""),
                            //    identityImage: this.input.dataUrlIdCard.replace(/^data:image\/(png|jpg|jpeg);base64,/, "")

                            //};

                            var url = this.$store.state.envUrl + '/api/User/Ekyc'
                            //var url = process.env.VUE_APP_APPMAN_EKYC_URL
                            axios.post(url, body, {
                                headers: {
                                    'Content-Type': 'application/json',
                                    //'x-api-key': process.env.VUE_APP_APPMAN_X_API_KEY
                                }
                            }).then((response) => {
                                var result = response.data;
                                let resultCompare = (result.compare.result[0].similarity * 100).toFixed(2);

                                this.isLoading = false;
                                if (resultCompare < 70) {
                                    this.$swal({
                                        icon: 'error',
                                        text: 'รูปถ่ายใบหน้าไม่ใช่คนเดียวกับบัตรประจำตัวประชาชนกรุณาลองใหม่อีกครั้ง',
                                        title: 'ผิดพลาด',
                                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                                        showCancelButton: false,
                                        showDenyButton: false,

                                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                        confirmButtonColor: '#5c2e91',
                                        willClose: () => {
                                            this.$refs.pondFace.removeFiles()
                                        }
                                    })
                                }
                            }).catch(() => {
                                this.isLoading = false;
                                this.$swal({
                                    icon: 'error',
                                    text: 'เนื่องจากอัพโหลดไม่สำเร็จ หรือไม่ใช่รูปถ่ายใบหน้า กรุณาลองใหม่',
                                    title: 'ผิดพลาด',
                                    showCancelButton: false,
                                    showDenyButton: false,
                                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                    confirmButtonColor: '#5c2e91',
                                    willClose: () => {
                                        this.$refs.pondFace.removeFiles()
                                    }
                                })
                            });

                        }
                        img.src = fileDataUrl
                    }
                }
            },
            getPrefixes() {
                var url = this.$store.state.envUrl + '/api/Master/Prefix';
                axios.get(url)
                    .then((response) => {
                        this.prefixes = response.data;
                        this.isLoading = false
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
        },
        
        created() {

            if (process.env.NODE_ENV == "production") {
                //--Publish--

                this.os = liff.getOS()

                //if (liff.getOS() == "android") {
                //    await liff.init({
                //        liffId: this.$store.state.liffId
                //    }).then(() => {
                //        if (liff.isLoggedIn()) {
                //            liff.getProfile().then(profile => {
                //                this.$store.state.userTokenLine = profile.userId
                //            }).catch(err => alert(err));
                //        } else {
                //            liff.login();
                //            liff.getProfile().then(profile => {
                //                this.$store.state.userTokenLine = profile.userId
                //            }).catch(err => alert(err));
                //        }

                //    }).catch(err => {
                //        alert(err);
                //        throw err
                //    });
                //}

            } else if (process.env.NODE_ENV == "development") {
                //--LocalHost--
                this.$store.state.userTokenLine = "U097368892fbcd4c33f07fcd4d069Mock";
                this.os = liff.getOS()
            }
            this.getPrefixes()

        }
    };
</script>

<style>
    .star-require {
        color: red;
    }

    .btn-add-img-ocr {
        height: 65px;
        border-radius: 5px;
        width: 100%;
    }

    .btn-change-img-ocr {
        border-radius: 5px;
        width: 100%;
    }

    .style-chooser .vs__search::placeholder,
    .style-chooser .vs__dropdown-toggle,
    .style-chooser .vs__dropdown-menu {
        background: #e1deec;
        border: none;
        color: #394066;
        text-transform: lowercase;
        font-variant: small-caps;
        border-radius: 7px;
        font-size: 13px;
    }

    .select2-dropdown {
        background-color: #e1deec;
        border: none;
        border-radius: 7px;
        outline: none;
        font-size: 13px;
    }

    .form-label-ocr {
        margin-bottom: 0px;
        margin-left: 5px;
    }

    .form-flex {
        display: flex;
    }

    .vs-checkbox-mask::before {
        background-color: white;
    }
</style>