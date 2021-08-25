<template>
    <div class="container" align="center">

        <h2 id="header2" class="">ยืนยันการส่งคำร้อง</h2>
        <div id="app" class="container mt-5">
            <p>
                กรุณากดขอรหัส OTP เพื่อรับรหัสยืนยันการส่งคำร้อง
            </p>
            <div class="row">
                <div class="col-7 mb-5">
                    <!--<div class="card card-tel">
                        <div class="position-icon mt-2">
                            <ion-icon name="call-outline"></ion-icon>
                            <label class="lbl-tel">xxx-xxx-9898</label>
                        </div>
                    </div>-->
                    <b-form-input class="mb-3" type="text" placeholder="xxx-xxx-9898" v-model="mockTel" :maxlength="10" />
                </div>
                <div class="col-5 mb-5">
                    <button class="btn-request-otp" type="button" @click="requestOTP" :disabled="disableBtnReqOTP">ขอรหัส OTP</button>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <p class="space-title mt-5">
                        กรุณากรอกรหัส OTP ที่ส่งไปยัง SMS
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div style="display: flex; flex-direction: row; margin-left: 10px">
                        <v-otp-input ref="otpInput"
                                     input-classes="otp-input"
                                     separator="-"
                                     :num-inputs="6"
                                     :should-auto-focus="true"
                                     :is-input-num="true"
                                     @on-change="handleOnChange"
                                     @on-complete="handleOnComplete" />

                        <!--<button @click="handleClearInput()">Clear Input</button>-->
                    </div>
                    <!--<div class="vue-otp-2">
                        <div v-for="(v,i) in otpLength * 2 - 1" :key="i/2">
                            <input v-if="i%2 === 0"
                                   :ref="'input' + (i/2)"
                                   @keyup="handleInput($event, i/2)"
                                   v-model="otp[i/2]"
                                   minlength="1"
                                   maxlength="1"
                                   @focus="handleFocus($event, i/2)" />

                            <span v-if="i%2 !== 0 && true">{{character}}</span>
                        </div>
                    </div>-->
                </div>
            </div>
            <div class="row">
                <div class="col-6  pt-2">
                    <p class=" pink-title fw-bold text-start " @click="requestOTP" v-if="countDown == 20">
                        <ion-icon name="reload-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>ขอรหัสอีกครั้ง
                    </p>
                </div>
                <div class="col-6  pt-2">
                    <p class=" black-title fw-bold text-start">
                        <ion-icon name="time-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>00:{{countDown}} น.
                    </p>
                </div>
            </div>
            <div>
                <br>
                <button class="btn-confirm-money" type="button" @click="submit">ยืนยันการส่งคำร้อง</button>
            </div>
        </div>
    </div>
</template>

<style>
    .input-tel-otp {
        border-radius: 10px;
        background-color: gainsboro;
        font-size: 15px;
        margin-right: 1rem;
        padding: 0rem 1rem;
        font-weight: bold;
        width: 60%
    }

    .btn-request-otp {
        border-radius: 10px;
        padding: 7px 10px;
        background-color: #5c2e91;
        color: whitesmoke;
        border-style: none;
        font-size: 15px;
    }

    .input-number-otp {
        margin: 1rem 3% 0rem 3%;
        max-width: 2rem;
        border-radius: 10px;
        font-size: 15px;
        border-style: groove;
    }

    .pink-title {
        color: hotpink;
        font-size: 13px;
        font-weight: bold;
        margin: 0 5% 0 5%;
    }

        .pink-title:hover {
            cursor: pointer;
        }

    .black-title {
        color: dimgray;
        font-size: 12px;
        padding: 0% 0%;
        float: right;
        margin: 0 2% 0 2%;
        font-weight: bold;
    }

    .lbl-tel {
        font-size: 13px;
        text-align: start;
        font-weight: bold;
        color: black;
        padding-left: 10px;
    }

    .card-tel {
        padding: 0 1rem;
        border: 1px solid;
        display: block;
        float: left;
        margin-left: 2%;
        width: 55%;
        height: 100%;
        background-color: gainsboro;
        border-radius: 10px;
    }

    .position-icon {
        float: left;
        padding-top: 0%;
        color: deeppink;
        font-size: 20px;
        text-align: start;
    }

    .mb-3.form-control {
        background-color: white;
        border-radius: 10px;
        padding: 5px 10px;
        margin-top: -1px;
        box-shadow: 0 2px var(--main-color);
        transform: translateY(2px);
        border: none;
    }

    .otp-input {
        width: 60%;
        height: 40px;
        padding: 0;
        margin: 0 5px;
        font-size: 14px;
        border-radius: 10px;
        border: 1px solid rgba(0, 0, 0, 0.3);
        text-align: center;
        -webkit-text-security: disc;
    }

        .otp-input.error {
            border: 1px solid red !important;
        }

        .otp-input::-webkit-inner-spin-button,
        .otp-input::-webkit-outer-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }

    .swal2-title {
        font-family: 'Mitr';
        font-size: 18px;
    }

    .swal2-html-container {
        font-family: 'Mitr';
        font-size: 15px;
    }
</style>

<script>
    import axios from 'axios'
    import qs from 'qs'
   
   
    export default {
        data() {
            return {
                msg: 'ยืนยันการส่งคำร้อง',
                accData: this.$store.getters.accGetter(this.$route.params.id),
                dataOTP: { token: "", ref_code: "" },
                inputOTP: "",
                verifyResultOTP: { status: "" },
                countDown: 20,
                disableBtnReqOTP: false,
                mockTel: ""
            }
        },
        methods: {
            countDownTimer() {
                if (this.countDown > 0) {
                    this.disableBtnReqOTP = true
                    setTimeout(() => {
                        this.countDown -= 1
                        this.countDownTimer()
                    }, 1000)
                } else if (this.countDown <= 0) {
                    this.disableBtnReqOTP = false
                    this.countDown = 20
                    
                }
            },

            requestOTP() {
                
                //var tel = "";
                const body = { 'TelNo': this.mockTel };
                console.log(qs.stringify(body))
                axios.post('https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/OTP/RequestOTP', qs.stringify(body), {
                    headers: {
                        // Overwrite Axios's automatically set Content-Type
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).then((response) => {
                    this.countDownTimer()
                    this.dataOTP = response.data.result
                    console.log(this.dataOTP);

                }).catch(function (error) {
                    this.$swal({
                        icon: 'error',
                        text: 'กรุณากดปุ่มขอรหัสยืนยัน OTP อีกครัง',
                        title: 'ผิดพลาด',
                        showCancelButton: false,
                        showDenyButton: true,
                        showConfirmButton: false,
                        denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        denyButtonColor: '#dad5e9',
                    })
                    console.log(error);
                });              
            },
            
            submit: async function () {
                this.$swal({
                    title: 'กำลังตรวจสอบ',
                    html: 'ขณะนี้ระบบกำลังตรวจสอบรหัสยืนยัน OTP',
                    timerProgressBar: true,
                    didOpen: () => {
                        this.$swal.showLoading()

                    },
                    willClose: () => {

                    }
                })
                await this.verifyOTP()
            },
            verifyOTP() {
                const body = {
                    'token': this.dataOTP.token,
                    'otp_code': this.inputOTP,
                    'ref_code': this.dataOTP.ref_code
                };
               
                console.log(qs.stringify(body))
                axios.post('https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/OTP/VerifyOTP', qs.stringify(body), {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).then((response) => {
                    this.verifyResultOTP = response.data
                    console.log(this.verifyResultOTP.status);
                    if (this.verifyResultOTP.status == "false") {
                        this.$swal.close();
                        this.$swal({
                            icon: 'error',
                            text: 'รหัสยืนยัน OTP ไม่ถูกต้องกรุณาลองใหม่อีกครั้ง',
                            title: 'ผิดพลาด',
                            showCancelButton: false,
                            showDenyButton: true,
                            showConfirmButton: false,
                            denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            denyButtonColor: '#dad5e9',

                        })
                    } else if (this.verifyResultOTP.status == "true") {
                        this.$swal.close();
                        this.showSwal()
                    }
                }).catch(function (error) {
                    console.log(error);
                });
            },
            
            showSwal() {                
                this.$swal({
                    icon: 'success',
                    text: 'ท่านสามารถติดตามผลดำเนินการได้ที่เมนูติดตามสถานะ',
                    title: 'ส่งคำร้องเรียบร้อยแล้ว',
                    /*footer: '<a href="">Why do I have this issue?</a>'*/
                    showCancelButton: false,
                    showDenyButton: true,
                    denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    denyButtonColor: '#dad5e9',
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ติดตามสถานะ",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'Accident' })
                    }
                }).then((result) => {

                    if (result.isConfirmed) {
                        this.$router.push({ name: 'CheckStatus' })
                    } else if (result.isDenied) {
                        this.$router.push({ name: 'Accident' })
                    }
                });
            },
            handleOnComplete(value) {
                console.log('OTP completed: ', value);
                this.inputOTP = value
            },
            handleOnChange(value) {
                console.log('OTP changed: ', value);
            },
            handleClearInput() {
                this.$refs.otpInput.clearInput();
            },
            

        }
        
    }
</script>
