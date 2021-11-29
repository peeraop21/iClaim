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
                    ข้าพเจ้ายินยอมให้ใช้ข้อมูลส่วนบุคคลในการขอใช้สิทธิ์เบิกค่าเสียหายเบื้องต้น
                </p>
            </div>
        </div>
        <div v-if="acceptData">
            <div class="box-container space-contianer px-2">
                <div class="row" align="center">
                    <div class="col-12">
                        <div class="form-control-sm">
                            <label class="form-label title-advice-menu">อัพโหลดรูปภาพเพื่อยืนยันตัวตน</label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
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
                    <!--<div class="col-6">
                        <file-pond credits="null"
                                   label-idle="รูปถ่ายหน้าเซลฟี"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-bind:files="faceFile"
                                   v-on:addfile="onAddfaceFile" />
                    </div>-->
                </div>

            </div>
            <b-overlay :show="isOverlay" rounded="sm">
                <div class="box-container  space-contianer px-2 mt-2 mb-5">
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
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.idCardNo.$model" type="number" :class="{ 'is-invalid': $v.input.idCardNo.$error }"></b-form-input>
                            <div v-if="submitted && !$v.input.idCardNo.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเลขบัตรประชาชน</div>
                        </div>
                        <div class="col-6" style="padding-left:5px">
                            <label class="form-label-ocr">รหัสหลังบัตร<span class="star-require">*</span></label>
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.idcardLaserCode.$model" :class="{ 'is-invalid': $v.input.idcardLaserCode.$error }"></b-form-input>
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
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.firstname.$model" :class="{ 'is-invalid': $v.input.firstname.$error }"></b-form-input>
                            <div v-if="submitted && !$v.input.firstname.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกชื่อ</div>
                        </div>
                        <div class="col-4 " style="padding-left:0px">
                            <label class="form-label-ocr">นามสกุล<span class="star-require">*</span></label>
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.lastname.$model" :class="{ 'is-invalid': $v.input.lastname.$error }"></b-form-input>
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
                            <b-form-input class="mt-0 mb-2" placeholder="" v-model="$v.input.telNo.$model" type="number" :class="{ 'is-invalid': $v.input.telNo.$error }"></b-form-input>
                            <div v-if="submitted && !$v.input.lastname.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเบอร์โทร</div>
                        </div>
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


    </div>
</template>

<script>

    import * as moment from "moment/moment";
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

    import Loading from 'vue-loading-overlay';

    import { required } from "vuelidate/lib/validators";

    // Create component
    const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview, FilePondPluginFileEncode);


    //Your Javascript lives within the Script Tag
    export default {
        name: "Ocr",
        components: {
            Loading,
            FilePond
        },
        data() {
            return {
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
                input: { idCardNo: null, idcardLaserCode: null, prefix: null, firstname: null, lastname: null, dateBirth: null, telNo: null, base64IdCard: null, base64Face: null },
                resultOCR: {},
                acceptData: false,
                prefixes: null,
                isLoading: true,
                submitted: false,
                isOverlay:true
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
                if (this.input.base64IdCard == null) {
                    this.$swal({
                        icon: 'warning',
                        text: 'กรุณาอัพโหลดรูปบัตรประจำตัวประชาชน',                       
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,

                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                    })
                    return false;
                }
                this.submitted = true;

                // stop here if form is invalid
                this.$v.$touch();
                if (this.$v.$invalid) {
                    return false;
                }
                this.isLoading = true;
                await this.postUserData()
            },
            postUserData() {

                let body = {
                    idcardNo: this.input.idCardNo,
                    idcardLaserCode: this.input.idcardLaserCode,
                    prefix: this.input.prefix,
                    fname: this.input.firstname,
                    lname: this.input.lastname,
                    stringDateofBirth: this.input.dateBirth,
                    mobileNo: this.input.telNo,
                    lineId: this.$store.state.userTokenLine,
                    base64IdCard: this.input.base64IdCard
                };
                console.log(body)
                axios.post(this.$store.state.envUrl + "/api/User", JSON.stringify(body), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then((response) => {
                    this.isLoading = false;
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
                }).catch((error) => {
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


            onAddidCardFile: async function (error, file) {
                this.isLoading = true;
                this.input.base64IdCard = file.getFileEncodeBase64String()
                this.isOverlay = false;
                //this.idCardTest.file = file
                //this.idCardTest.filename = file.getFileEncodeDataURL()

                //this.ImageData.IdCardBase64 = this.idCardTest.base64;

                //console.log(file);
                //this.$swal({
                //    title: 'กำลังอ่านข้อมูล',
                //    html: 'ขณะนี้ระบบกำลังอ่านข้อมูลบัตรประจำตัวประชาชน',
                //    timerProgressBar: true,
                //    didOpen: () => {
                //        this.$swal.showLoading()

                //    },
                //    willClose: () => {

                //    }
                //})
                var url = 'https://ml.appman.co.th/v1/thailand-id-card/front'
                var bodyFormData = new FormData();
                bodyFormData.append('Username', 'rvp.user0001');
                bodyFormData.append('Password', 'Rvp@2021')
                bodyFormData.append('image', file.file);

                axios.post(url, bodyFormData, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        'x-api-key': 'uEj0XAy4y69YPgK6IPTFnaVZn7ZsYaum9gJBWowg'
                    }
                }).then((response) => {
                    this.resultOCR = response.data.result;
                    console.log("Result: ", this.resultOCR)
                    if (this.resultOCR != "") {
                        this.input.idCardNo = this.resultOCR.id_number.replaceAll(" ", "");
                        this.input.prefix = this.resultOCR.title_th;
                        this.input.firstname = this.resultOCR.first_name_th;
                        this.input.lastname = this.resultOCR.last_name_th;
                        this.input.dateBirth = moment(this.formatResultOcrDate(this.resultOCR.date_of_birth_en)).format("YYYY-MM-DD");
                        this.input.base64IdCard = file.getFileEncodeBase64String();
                        this.isLoading = false;
                    }
                }).catch(function (error) {
                    console.log(error);
                });
            },
            onAddfaceFile: async function (error, file) {
                console.log(file)

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
        mounted() {


        },
        async created() {
            if (process.env.NODE_ENV == "production") {
                //--Publish--
                await liff.init({
                    liffId: '1656611867-ylXVO2D8',
                }).then(() => {
                    if (liff.isLoggedIn()) {
                        liff.getProfile().then(profile => {
                            this.$store.state.userTokenLine = profile.userId
                        }).catch(err => alert(err));
                    } else {
                        liff.login();
                        liff.getProfile().then(profile => {
                            this.$store.state.userTokenLine = profile.userId
                        }).catch(err => alert(err));
                    }

                }).catch(err => {
                    alert(err);
                    throw err
                });
            } else if (process.env.NODE_ENV == "development") {
                //--LocalHost--
                this.$store.state.userTokenLine = "U097368892fbcd4c33f07fcd4d069Mock";
            }
            this.getPrefixes()
        }
    };
</script>

<style>
    .star-require {
        color: red;
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