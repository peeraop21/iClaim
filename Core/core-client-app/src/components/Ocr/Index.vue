<template>
    <div class="container ">
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
                    ข้าพเจ้ายินยอมให้ใช้ข้อมูลส่วนบุคคลในการขอใช้สิทธิ์เบิกค่าเสียหายเบื้องต้น
                </p>
            </div>
        </div>
        <div v-if="acceptData">
            <div class="box-container space-contianer">
                <div class="row" align="center">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label title-advice-menu">อัพโหลดรูปภาพเพื่อยืนยันตัวตน</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <file-pond credits="null"
                                   label-idle="รูปบัตรประจำตัวประชาชน"
                                   capture="user"
                                   accept="image/*"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-bind:files="idCardFile"
                                   v-on:addfile="onAddidCardFile" />
                    </div>
                    <div v-if="ImageData.IdCardBase64 != null" class="col-6">
                        <file-pond credits="null"
                                   label-idle="รูปถ่ายหน้าเซลฟี"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-bind:files="faceFile"
                                   v-on:addfile="onAddfaceFile" />
                    </div>
                </div>

            </div>
            <div class="box-container  space-contianer">
                <div class="row" align="center">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label title-advice-menu">ข้อมูลจากรูปบัตรประจำตัวประชาชน</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label">เลขที่บัตรประจำตัว : {{dataIdCard.id}}</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label">ชื่อ-สกุล (ภาษาไทย) : {{dataIdCard.name_th}}</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label">ชื่อ-สกุล (ภาษาอังกฤษ) : {{dataIdCard.first_name_en}} {{dataIdCard.last_name_en}}</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label">วันเกิด (ค.ศ.) : {{dataIdCard.date_of_birth}}</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label">ศาสนา : {{dataIdCard.religion}}</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label">ที่อยู่ : {{dataIdCard.address}}</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label">วันที่ออกบัตร (ค.ศ.) : {{dataIdCard.date_of_issue}}</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label">วันที่หมดอายุ (ค.ศ.) : {{dataIdCard.date_of_expiry}}</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm form-flex">
                            <label class="form-label" style="min-width: 145px;">รหัสหลังบัตรประจำตัว :</label>
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="behindIdCardNo"></b-form-input>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-control-sm form-flex">
                            <label class="form-label" style="min-width: 205px ">เบอร์โทร (ใช้ในการขอรหัส OTP) :</label>
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="telNo"></b-form-input>
                        </div>
                    </div>
                </div>


            </div>
            <div class="row">
                <div class="col-12">
                    <button class="btn-next-submit" style="width: 100%; padding: 8px 0px;" @click="submit">ดำเนินการต่อ</button>
                </div>
            </div>
            <br>
            <br>
        </div>


    </div>
</template>

<script>


    import liff from '@line/liff'

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



    // Create component
    const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview, FilePondPluginFileEncode);


    //Your Javascript lives within the Script Tag
    export default {
        name: "Ocr",
        components: {

            FilePond
        },
        data() {
            return {
                /*dataCard: { address: "", date_of_birth: "", date_of_expiry: "", date_of_issue: "", first_name_en: "", id: "", last_name_en: "", name_th: "", religion: "", serial_number: "" },*/
                telNo: "",
                behindIdCardNo: "",
                dataIdCard: {},
                scoreCompare: {},
                idCardFile: null,
                faceFile: null,
                idCardTest: { file: null, filename: "", base64: "" },
                faceTest: { file: null, filename: "", base64: "" },
                SECRET_ID: "cc425efa2ec80be4c1149f0642c9ff94",
                SECRET_KEY: "d387810e33e19128c8cc6643c7758143408d6359",
                URI: "/v1/compare-face-id-card",
                URL: "https://iai.flashsoftapi.com/v1/compare-face-id-card",
                acceptData: false,
                ImageData: { IdCardBase64: null, FaceBase64: null },
                uploadFaceError:null,
                result: ""
            };
        },

        methods: {

            submit: async function () {
                if (this.ImageData.IdCardBase64 == null) {
                    this.$swal({
                        icon: 'error',
                        text: 'กรูณาอัพโหลดรูปถ่ายบัตรประจำตัวประชาชน',
                        title: 'ผิดพลาด',
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                    })
                } else if (this.ImageData.FaceBase64 == null) {
                    this.$swal({
                        icon: 'error',
                        text: 'กรูณาอัพโหลดรูปถ่ายหน้าเซลฟี เพื่อทำการเปรียบเทียบใบหน้า',
                        title: 'ผิดพลาด',
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                    })
                } else if (this.behindIdCardNo == "" && this.telNo == "") {
                    this.$swal({
                        icon: 'error',
                        text: 'กรูณากรอกข้อมูลรหัสหลังบัตรประจำตัว และเบอร์โทร',
                        title: 'ผิดพลาด',
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                    })
                } else if (this.behindIdCardNo == "" && this.telNo != "") {
                    this.$swal({
                        icon: 'error',
                        text: 'กรูณากรอกข้อมูลรหัสหลังบัตรประจำตัว',
                        title: 'ผิดพลาด',
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                    })
                } else if (this.behindIdCardNo != "" && this.telNo == "") {
                    this.$swal({
                        icon: 'error',
                        text: 'กรูณากรอกข้อมูลเบอร์โทร',
                        title: 'ผิดพลาด',
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                    })
                } else if (this.scoreCompare != null && this.scoreCompare.score < 70) {

                    this.$swal({
                        icon: 'error',
                        text: 'ผลการเทียบใบหน้ากับบัตรประจำตัวประชาชนตรงกันต่ำกว่า 90%' + ' กรุณาอัพโหลดรูปใหม่เพื่อเปรียบเทียบอีกครั้ง',
                        title: 'ผิดพลาด',
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                    })
                } else if (this.scoreCompare != null && this.scoreCompare.score >= 70) {
                    this.$swal({
                        title: 'กำลังประมวลผล',
                        html: 'ขณะนี้ระบบกำลังประมวลผล และบันทึกข้อมูล',
                        timerProgressBar: true,
                        didOpen: () => {
                            this.$swal.showLoading()

                        },
                        willClose: () => {

                        }
                    })
                    await this.postUserData();

                } else if (this.uploadFaceError != null) {
                    this.$swal({
                        icon: 'error',
                        text: 'กรูณาอัพโหลดรูปถ่ายหน้าเซลฟีอีกครั้ง เพื่อทำการเปรียบเทียบใบหน้า',
                        title: 'ผิดพลาด',
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                    })
                }
                 
                    
                 
            },
            postUserData() {
                let address = this.dataIdCard.address.split(/[\s,]+/)

                let splitMoo = null;

                console.log("splitMoo : ", splitMoo);
                let homeid = null;
                let hmo = null;
                if (this.dataIdCard.address.includes("หมู่ที่")) {
                    splitMoo = this.dataIdCard.address.split("หมู่ที่ ")[1];
                    hmo = splitMoo.charAt(0);
                }
                let road = null;
                let tumbol = null;
                let amphur = null;
                let changwat = null;

                for (let i = 0; i < address.length; i++) {
                    if (address[i].includes("ต.")) {
                        tumbol = address[i].replace("ต.", "")
                    } else if (address[i].includes("แขวง")) {
                        tumbol = address[i].replace("แขวง", "")
                    } else if (address[i].includes("อ.")) {
                        amphur = address[i].replace("อ.", "")
                    } else if (address[i].includes("เขต")) {
                        amphur = address[i].replace("เขต", "")
                    } else if (address[i].includes("จ.")) {
                        changwat = address[i].replace("จ.", "")
                    } else if (i == (address.length - 1)) {
                        changwat = address[i]
                    } else if (address[i].includes("ถ.")) {
                        road = address[i].replace("ถ.", "")
                    } else if (i == 1) {
                        homeid = address[i]
                    }

                }
                let body = {
                    idcardNo: this.dataIdCard.id,
                    prefix: this.dataIdCard.name_th.split(" ", 1)[0],
                    fname: this.dataIdCard.name_th.split(" ", 2)[1],
                    lname: this.dataIdCard.name_th.split(" ", 3)[2],
                    stringDateofBirth: this.dataIdCard.date_of_birth,
                    homeHouseNo: homeid,
                    homeHmo: hmo,
                    homeRoad: road,
                    homeTumbolId: tumbol,
                    homeCityId: amphur,
                    homeProvinceId: changwat,
                    idcardLaserCode: this.behindIdCardNo,
                    mobileNo: this.telNo,
                    lineId: this.$store.state.userTokenLine
                };
                console.log(body)
                axios.post("/api/User", JSON.stringify(body), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then((response) => {
                        this.$swal.close();
                        this.$swal({
                            icon: 'success',
                            text: 'ระบบได้ทำการบันทึกข้อมูลการยืนยันตัวตนของท่านเรียบร้อยแล้ว',
                            title: 'ยืนยันตัวตนสำเร็จ',
                            /*footer: '<a href="">Why do I have this issue?</a>'*/
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ดำเนินการต่อ",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                                this.$store.state.hasRegistered = true;
                                /*this.$router.push({ name: 'Accident' })*/
                                liff.closeWindow()
                            }
                        })

                        console.log(response.data)
                    })
                    .catch(function (error) {
                        console.log(error);
                        this.$swal({
                            icon: 'error',
                            text: 'บันทึกข้อมูลไม่สำเร็จกรุณาลองใหม่อีกครั้ง',
                            title: 'ผิดพลาด',
                            /*footer: '<a href="">Why do I have this issue?</a>'*/
                            showCancelButton: false,
                            showDenyButton: false,

                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                        })
                    });
            },
            postIdCardImage() {

                axios.post("/api/Ocr/IdCard", JSON.stringify(this.ImageData), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then((response) => {
                        this.dataIdCard = response.data.responseOcrIdCard;
                        console.log(this.dataIdCard)
                        if (this.dataIdCard != "") {
                            this.$swal.close();
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            postFaceImage() {

                axios.post("/api/Ocr/CompareFace", JSON.stringify(this.ImageData), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then((response) => {
                        this.scoreCompare = response.data.resultCompare;
                        console.log("score! = " ,response.data.resultCompare)
                        this.uploadFaceError = response.data.errorContent;
                        console.log(this.uploadFaceError)
                        if (this.uploadFaceError != null) {
                            this.$swal.close();
                            this.$swal({
                                icon: 'error',
                                text: this.uploadFaceError,
                                title: 'ผิดพลาด',
                                /*footer: '<a href="">Why do I have this issue?</a>'*/
                                showCancelButton: false,
                                showDenyButton: false,

                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                            })
                        }
                        else if (this.scoreCompare != "") {
                            this.$swal.close();
                            this.$swal({
                                icon: 'success',
                                text: /*'ผลการเทียบใบหน้ากับบัตรประจำตัวประชาชนตรงกัน ' + this.scoreCompare.score + "%"*/"ผลการเปรียบเทียบรูปถ่ายใบหน้ากับบัตรประจำตัวประชาชนเป็นคนเดียวกัน",
                                title: 'ตรวจสอบสำเร็จ',
                                /*footer: '<a href="">Why do I have this issue?</a>'*/
                                showCancelButton: false,
                                showDenyButton: false,

                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                            })
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            onAddidCardFile: async function (error, file) {
                this.idCardTest.file = file
                this.idCardTest.filename = file.getFileEncodeDataURL()
                this.idCardTest.base64 = file.getFileEncodeBase64String()
                this.ImageData.IdCardBase64 = this.idCardTest.base64;

                console.log(file);
                this.$swal({
                    title: 'กำลังอ่านข้อมูล',
                    html: 'ขณะนี้ระบบกำลังอ่านข้อมูลบัตรประจำตัวประชาชน',
                    timerProgressBar: true,
                    didOpen: () => {
                        this.$swal.showLoading()

                    },
                    willClose: () => {

                    }
                })
                await this.postIdCardImage();
                /*await this.readIdCard()*/


            },
            onAddfaceFile: async function (error, file) {
                this.faceTest.file = file
                this.faceTest.filename = file.getFileEncodeDataURL()
                this.faceTest.base64 = file.getFileEncodeBase64String()
                this.ImageData.FaceBase64 = this.faceTest.base64
                this.$swal({
                    title: 'กำลังตรวจสอบ',
                    html: 'ขณะนี้ระบบกำลังตรวจสอบใบหน้าเทียบกับบัตรประจำตัวประชาชน',
                    timerProgressBar: true,
                    didOpen: () => {
                        this.$swal.showLoading()

                    },
                    willClose: () => {

                    }
                })
                await this.postFaceImage()

            },





        },
        mounted() {
            

        },
        created() {
            
            
            
        }
    };
</script>

<style>
    .form-flex {
        display: flex;
    }

    .btn-next-submit {
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 20px;
        font-size: 15px;
        border: none;
    }

    .vs-checkbox-mask::before {
        background-color: white;
    }
</style>