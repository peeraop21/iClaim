<template>
    <div class="container" align="center">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>
        <h2 id="header2" class="">ยืนยันการ{{fromText}}</h2>
        <div id="app" class="container mt-5">
            <p>
                กรุณากดขอรหัส OTP เพื่อรับรหัสยืนยันการ{{fromText}}
            </p>
            <div class="row">
                <div class="col-7 mb-5">
                    <b-form-input class="mb-3" type="text" :placeholder="displayMaskTelNo" v-model="mockTel" :maxlength="10" disabled />
                </div>
                <div class="col-5 mb-5">
                    <button class="btn-request-otp" type="button" @click="requestOTP" v-bind:disabled="disableBtnReqOTP">ขอรหัส OTP</button>
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

                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-6  pt-2">
                    <p class=" pink-title fw-bold text-start " @click="requestOTP" v-if="countDown == 120">
                        <ion-icon name="reload-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>ขอรหัสอีกครั้ง
                    </p>
                </div>
                <div class="col-6  pt-2">
                    <p class=" black-title fw-bold text-start">
                        <ion-icon name="time-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>{{displayCountDown}} น.
                    </p>
                </div>
            </div>
            <div v-if="inputOTP.length > 5">
                <br>
                <button class="btn-confirm-money" type="button" @click="submit">ยืนยันการ{{fromText}}</button>
                <!--<button class="btn-confirm-money" type="button" @click="countDownTimer">TestCountDown</button>-->
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
    //import liff from '@line/liff'
    import mixin from '../../mixin/index.js'
    import axios from 'axios'
    import qs from 'qs'
    import watermark from 'watermarkjs'
    import Loading from 'vue-loading-overlay';


    export default {
        mixins: [mixin],
        components: {
            Loading
        },
        data() {
            return {
                msg: 'ยืนยันการส่งคำร้อง',
                accData: null,
                userData: null,
                dataOTP: { token: "", ref_code: "" },
                inputOTP: "",
                verifyResultOTP: { status: "" },
                countDown: 120,
                displayCountDown: "02:00",
                disableBtnReqOTP: false,
                mockTel: "",
                isLoading: true,
                fromText: "",
                displayMaskTelNo: "",

            }
        },
        methods: {
            countDownTimer() {
                if (this.countDown > 0) {
                    if (this.countDown < 120 && this.countDown > 60) {
                        this.disableBtnReqOTP = true
                        setTimeout(() => {
                            this.countDown -= 1
                            this.displayCountDown = (this.countDown < 70 && this.countDown >= 60) ? "01:0" + (this.countDown - 60) : "01:" + (this.countDown - 60)
                            this.countDownTimer()
                        }, 1000)
                    } else if (this.countDown <= 60) {
                        this.disableBtnReqOTP = true
                        setTimeout(() => {
                            this.countDown -= 1
                            this.displayCountDown = (this.countDown < 10 && this.countDown >= 0) ? "00:0" + (this.countDown) : "00:" + (this.countDown)
                            this.countDownTimer()
                        }, 1000)
                    } else {
                        this.disableBtnReqOTP = true
                        setTimeout(() => {
                            this.countDown -= 1
                            this.displayCountDown = "01:" + (this.countDown - 60)
                            this.countDownTimer()
                        }, 1000)
                    }

                } else if (this.countDown <= 0) {
                    this.displayCountDown = "02:00"
                    this.disableBtnReqOTP = false
                    this.countDown = 120

                }
            },

            postData() {
                if (this.$route.params.from == "Edit") {
                    if (this.$store.state.inputApprovalData.BillsData != null) {


                        for (let i = 0; i < this.$store.state.inputApprovalData.BillsData.length; i++) {
                            this.$store.state.inputApprovalData.BillsData[i].money = this.$store.state.inputApprovalData.BillsData[i].money.toString()
                        }
                    }
                    axios.post(this.$store.state.envUrl + "/api/Approval/UpdateApproval", JSON.stringify(this.$store.state.inputApprovalData), {
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': "Bearer " + this.$store.state.jwtToken.token
                        }
                    }).then(() => {
                        this.isLoading = false
                        this.$swal({
                            icon: 'success',
                            //text: 'ท่านสามารถติดตามผลดำเนินการได้ที่เมนูติดตามสถานะ',
                            title: 'ส่งเอกสารเพิ่มเติมแล้ว',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                                this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                            }
                        })
                        this.resetData();
                    }).catch((error) => {
                        this.isLoading = false;
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.postData()

                        }
                    });
                }
                else if (this.$route.params.from == "CanselApproval") {
                    axios.post(this.$store.state.envUrl + "/api/Approval/CanselApproval", JSON.stringify(this.$store.state.inputApprovalData), {
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': "Bearer " + this.$store.state.jwtToken.token
                        }
                    }).then(() => {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'success',
                            //text: 'ท่านสามารถติดตามผลดำเนินการได้ที่เมนูติดตามสถานะ',
                            title: 'ยกเลิกคำร้องเรียบร้อยแล้ว',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                                this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                            }
                        })
                        this.resetData();
                    }).catch((error) => {
                        this.isLoading = false;
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.postData()

                        }
                    });

                }
                else if (this.$route.params.from == "Create") {
                    axios.post(this.$store.state.envUrl + "/api/Approval", JSON.stringify(this.$store.state.inputApprovalData), {
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': "Bearer " + this.$store.state.jwtToken.token
                        }
                    }).then((response) => {
                        this.pushMessageToUser(response.data.reqNo)
                    }).catch((error) => {
                        this.isLoading = false;
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.postData()

                        }
                    });
                }
                else if (this.$route.params.from == "CreateUser") {

                    axios.post(this.$store.state.envUrl + "/api/User", JSON.stringify(this.$store.state.inputUserData), {
                        headers: {
                            'Content-Type': 'application/json',
                        }
                    }).then(() => {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'success',
                            text: 'ระบบได้ทำการบันทึกข้อมูลการยืนยันตัวตนของท่านเรียบร้อยแล้ว',
                            title: 'ยืนยันตัวตนสำเร็จ',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ดำเนินการต่อ",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                                this.$store.state.hasRegistered = true;
                                this.$router.push('/Accident')
                                //if (liff.getOS() == "ios") {
                                //    this.$store.state.hasRegistered = true;
                                //    this.$router.push('/Accident')
                                //} else {
                                //    this.$store.state.hasRegistered = true;
                                //    liff.closeWindow()
                                //    window.close();
                                //}

                            }
                        })

                    }).catch((error) => {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'error',
                            text: 'บันทึกข้อมูลไม่สำเร็จกรุณาลองใหม่อีกครั้ง',
                            title: 'ผิดพลาด',
                            showCancelButton: false,
                            showDenyButton: false,

                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                                if (error.toString().includes("401")) {
                                    this.getJwtToken()

                                }
                            }
                        })


                    });
                } else if (this.$route.params.from == "ConfirmMoney") {
                    const url = this.$store.state.envUrl + "/api/Approval/ConfirmMoney";
                    const body = {
                        AccNo: this.$route.params.accNo,
                        VictimNo: parseInt(this.$route.params.victimNo),
                        ReqNo: parseInt(this.$route.params.appNo),
                        UserIdCard: this.$store.state.userStateData.idcardNo
                    };
                    axios.post(url, JSON.stringify(body), {
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': "Bearer " + this.$store.state.jwtToken.token
                        }
                    }).then((response) => {
                        this.isLoading = false;
                        if (response.data == "Success") {
                            this.$swal({
                                icon: 'success',
                                title: 'ยืนยันรับจำนวนเงินเรียบร้อยแล้ว',
                                showCancelButton: false,
                                showDenyButton: false,
                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {
                                    this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                                }
                            })
                        }
                    }).catch((error) => {
                        this.isLoading = false;
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.postData()
                        }
                    });

                } else if (this.$route.params.from == "AccidentCreate") {
                    var url = this.$store.state.envUrl + '/api/accident/InsertAccident';
                    var apiConfig = {
                        headers: {
                            Authorization: "Bearer " + this.$store.state.jwtToken.token,
                            'Content-Type': 'application/json',
                        }
                    }
                    axios.post(url, JSON.stringify(this.$store.state.inputAccidentData), apiConfig)
                        .then((response) => {
                            console.log(response)
                            if (response.data.status == "Success") {
                                this.$swal({
                                    icon: 'success',
                                    title: 'ส่งข้อมูลอุบัติเหตุเพื่อขอใช้สิทธิ์สำเร็จ',
                                    showCancelButton: false,
                                    showDenyButton: false,
                                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                    confirmButtonColor: '#5c2e91',
                                    willClose: () => {
                                        this.$router.push('/Accident')
                                    }
                                })
                            }
                            this.isLoading = false;
                        })
                        .catch(function (error) {
                            this.isLoading = false;
                            alert(error);
                        });
                }
                


            },
            pushMessageToUser(reqNo) {
                const body = {
                    To: this.$store.state.userTokenLine,
                    Messages: "ท่านได้ทำการส่งคำร้องเรียบร้อยแล้ว เลขรับแจ้ง: " + this.$store.state.inputApprovalData.AccNo + " | เลขที่คำร้อง: " + reqNo + " ขณะนี้คำร้องอยู่ระหว่างตรวจสอบเอกสาร",
                    Type: "Text"
                };
                axios.post(this.$store.state.envUrl + "/api/PushMessage", JSON.stringify(body), {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': "Bearer " + this.$store.state.jwtToken.token
                    }
                }).then(() => {
                    this.isLoading = false;
                    this.resetData();
                    this.showSwal();

                }).catch(() => {
                    this.isLoading = false;
                    this.resetData();
                    this.showSwal();

                });
            },

            resetData() {
                this.$store.state.inputApprovalData.AccNo = null
                this.$store.state.inputApprovalData.VictimNo = null
                this.$store.state.inputApprovalData.AppNo = null
                this.$store.state.inputApprovalData.SumMoney = null
                this.$store.state.inputApprovalData.ClaimNo = null
                this.$store.state.inputApprovalData.Injury = null
                this.$store.state.inputApprovalData.BillsData = null
                this.$store.state.inputApprovalData.BankData = null
                this.$store.state.inputApprovalData.VictimData = null
                this.$store.state.inputApprovalData.RefCodeOtp = null

                this.$store.state.inputAccidentData.AccidentInput = null
                this.$store.state.inputAccidentData.AccidentCarInput = null
                this.$store.state.inputAccidentData.AccidentVictimInput = null

            },

            requestOTP() {
                this.isLoading = true
                //ตัวจริง
                const url = process.env.VUE_APP_OTP_REQUEST_URL;
                const body = {
                    TelNo: (this.$route.params.from == "CreateUser") ? this.$store.state.inputUserData.mobileNo.replaceAll("-", "") : this.userData.mobileNo
                };
                axios.post(url, qs.stringify(body), {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                    }
                }).then((response) => {
                    this.countDownTimer()
                    this.dataOTP = response.data.result
                    this.$store.state.inputApprovalData.RefCodeOtp = this.dataOTP.ref_code
                    this.$store.state.inputUserData.refCodeOtp = this.dataOTP.ref_code
                    this.isLoading = false

                }).catch(() => {
                    this.isLoading = false
                    this.$swal({
                        icon: 'error',
                        text: 'กรุณากดปุ่มขอรหัสยืนยัน OTP อีกครัง',
                        title: 'ผิดพลาด',
                        showCancelButton: false,
                        showDenyButton: true,
                        showConfirmButton: false,
                        denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        denyButtonColor: '#dad5e9'
                    });
                });
            },

            submit: async function () {
                this.isLoading = true
                await this.verifyOTP()
            },
            verifyOTP() {
                //ตัวจริง
                const url = process.env.VUE_APP_OTP_VERIFY_URL;
                const body = {
                    'token': this.dataOTP.token,
                    'otp_code': this.inputOTP,
                    'ref_code': this.dataOTP.ref_code
                };
                axios.post(url, qs.stringify(body), {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).then((response) => {
                    this.verifyResultOTP = response.data
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
                        this.isLoading = false
                    } else if (this.verifyResultOTP.status == "true") {
                        this.postData()
                    }
                }).catch(() => {
                    this.isLoading = false
                    this.$swal({
                        icon: 'error',
                        text: 'รหัสยืนยัน OTP ผิดพลาดกรุณากดขอรหัสใหม่ และกรอกใหม่อีกครั้ง',
                        title: 'ผิดพลาด',
                        showCancelButton: false,
                        showDenyButton: true,
                        showConfirmButton: false,
                        denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        denyButtonColor: '#dad5e9',

                    })
                });

            },

            showSwal() {
                this.$swal({
                    icon: 'success',
                    text: 'ท่านสามารถติดตามผลดำเนินการได้ที่เมนูติดตามสถานะ',
                    title: 'ส่งคำร้องเรียบร้อยแล้ว',
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
                        this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                    } else if (result.isDenied) {
                        this.$router.push('/Accident')
                    }
                });
            },
            handleOnComplete(value) {
                this.inputOTP = value
            },
            handleOnChange(value) {
                this.inputOTP = value
            },
            handleClearInput() {
                this.$refs.otpInput.clearInput();
            },
            stampWatermarksFromCreate() {
                //Stamp ลายน้ำ
                if (this.$store.state.inputApprovalData.BankData.bankFilename) {
                    var options1 = {
                        init(img) {
                            img.crossOrigin = 'anonymous';
                        }
                    };
                    var textBankRotate = function (target) {
                        var context = target.getContext('2d');
                        var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                        var x = (target.width / 2);
                        var y = (target.height / 2);
                        var fontSize = (target.width + target.height) * (3.5 / 100)
                        context.save();
                        context.translate(x, y);

                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                        } else {
                            context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                        }

                        context.restore();

                        var text1 = 'บริษัทกลางฯเท่านั้น';
                        context.translate(x, y);
                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                        } else {
                            context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                        }

                        return target;
                    };
                    var resultAddWatermarkToBankImage = "";
                    watermark([this.$store.state.inputApprovalData.BankData.bankBase64String], options1)
                        .image(textBankRotate)
                        .then((imgBank) => {
                            resultAddWatermarkToBankImage = imgBank.src;
                            resultAddWatermarkToBankImage = resultAddWatermarkToBankImage.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                            this.$store.state.inputApprovalData.BankData.bankBase64String = resultAddWatermarkToBankImage
                        }).catch(function (error) {
                            alert(error);
                        });
                } else {
                    this.$store.state.inputApprovalData.BankData.bankBase64String = this.$store.state.inputApprovalData.BankData.bankBase64String.replace(/^data:image\/(png|jpg|jpeg);base64,/, "")
                }
                

                for (let i = 0; i < this.$store.state.inputApprovalData.BillsData.length; i++) {
                    for (let j = 0; j < this.$store.state.inputApprovalData.BillsData[i].billFileShow.length; j++) {
                        var options = {
                            init(img) {
                                img.crossOrigin = 'anonymous';
                            }
                        };
                        var textRotate = function (target) {
                            var context = target.getContext('2d');
                            var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                            var x = (target.width / 2);
                            var y = (target.height / 2);
                            var fontSize = (target.width + target.height) * (3.5 / 100)
                            context.save();
                            context.translate(x, y);

                            if (target.width > target.height) {
                                context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                            } else {
                                context.rotate(-Math.atan(target.height / target.width));
                            }
                            context.globalAlpha = 0.3;
                            context.fillStyle = 'red';
                            context.font = fontSize + 'px Kanit';
                            context.textAlign = 'center';
                            if (target.width > target.height) {
                                context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                            } else {
                                context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                            }

                            context.restore();

                            var text1 = 'บริษัทกลางฯเท่านั้น';
                            context.translate(x, y);
                            if (target.width > target.height) {
                                context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                            } else {
                                context.rotate(-Math.atan(target.height / target.width));
                            }
                            context.globalAlpha = 0.3;
                            context.fillStyle = 'red';
                            context.font = fontSize + 'px Kanit';
                            context.textAlign = 'center';
                            if (target.width > target.height) {
                                context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                            } else {
                                context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                            }

                            return target;
                        };
                        var resultAddWatermark = "";
                        watermark([this.$store.state.inputApprovalData.BillsData[i].billFileShow[j]], options)
                            .image(textRotate)
                            .then((imgBill) => {
                                resultAddWatermark = imgBill.src;
                                resultAddWatermark = resultAddWatermark.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                                this.$store.state.inputApprovalData.BillsData[i].billFileShow[j] = resultAddWatermark
                            }).catch(function (error) {
                                alert(error);
                            });
                    }
                    
                }
                this.isLoading = false
            },
            stampWatermarksFromEditApproval() {
                //Stamp ลายน้ำ
                if (this.$store.state.inputApprovalData.BankData.isEditBankImage) {
                    var options1 = {
                        init(img) {
                            img.crossOrigin = 'anonymous';
                        }
                    };
                    var textBankRotate = function (target) {
                        var context = target.getContext('2d');
                        var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                        var x = (target.width / 2);
                        var y = (target.height / 2);
                        var fontSize = (target.width + target.height) * (3.5 / 100)
                        context.save();
                        context.translate(x, y);

                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                        } else {
                            context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                        }

                        context.restore();

                        var text1 = 'บริษัทกลางฯเท่านั้น';
                        context.translate(x, y);
                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                        } else {
                            context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                        }

                        return target;
                    };
                    var resultAddWatermarkToBankImage = "";
                    watermark([this.$store.state.inputApprovalData.BankData.bankBase64String], options1)
                        .image(textBankRotate)
                        .then((imgBank) => {
                            resultAddWatermarkToBankImage = imgBank.src;
                            resultAddWatermarkToBankImage = resultAddWatermarkToBankImage.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                            this.$store.state.inputApprovalData.BankData.bankBase64String = resultAddWatermarkToBankImage
                        }).catch(function (error) {
                            alert(error);
                        });
                }


                if (this.$store.state.inputApprovalData.BillsData != null) {
                    for (let i = 0; i < this.$store.state.inputApprovalData.BillsData.length; i++) {
                        if (this.$store.state.inputApprovalData.BillsData[i].isEditImage) {
                            for (let j = 0; j < this.$store.state.inputApprovalData.BillsData[i].editBillImage.length; j++) {
                                var options = {
                                    init(img) {
                                        img.crossOrigin = 'anonymous';
                                    }
                                };

                                watermark([this.$store.state.inputApprovalData.BillsData[i].editBillImage[j]], options)
                                    .dataUrl(function (target) {
                                        var context = target.getContext('2d');
                                        var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                                        var x = (target.width / 2);
                                        var y = (target.height / 2);
                                        var fontSize = (target.width + target.height) * (3.5 / 100)
                                        context.save();
                                        context.translate(x, y);

                                        if (target.width > target.height) {
                                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                                        } else {
                                            context.rotate(-Math.atan(target.height / target.width));
                                        }
                                        context.globalAlpha = 0.3;
                                        context.fillStyle = 'red';
                                        context.font = fontSize + 'px Kanit';
                                        context.textAlign = 'center';
                                        if (target.width > target.height) {
                                            context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                                        } else {
                                            context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                                        }

                                        context.restore();

                                        var text1 = 'บริษัทกลางฯเท่านั้น';
                                        context.translate(x, y);
                                        if (target.width > target.height) {
                                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                                        } else {
                                            context.rotate(-Math.atan(target.height / target.width));
                                        }
                                        context.globalAlpha = 0.3;
                                        context.fillStyle = 'red';
                                        context.font = fontSize + 'px Kanit';
                                        context.textAlign = 'center';
                                        if (target.width > target.height) {
                                            context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                                        } else {
                                            context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                                        }

                                        return target;
                                    })
                                    .then((imgBill) => {
                                        this.$store.state.inputApprovalData.BillsData[i].editBillImage[j] = imgBill.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                                    }).catch(function (error) {
                                        alert(error);
                                    });
                            }
                            
                        }

                    }
                }
            },

            stampWatermarksIdCardImage(imgDataUrl) {
                //Stamp ลายน้ำ
                var idCardOptions = {
                    init(img) {
                        img.crossOrigin = 'anonymous';
                    }
                };
                /*var resultAddWatermarkToIdCardImage = "";*/
                watermark([imgDataUrl], idCardOptions)
                    .dataUrl(function (target) {
                        var context = target.getContext('2d');
                        var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                        var x = (target.width / 2);
                        var y = (target.height / 2);
                        var fontSize = (target.width + target.height) * (3.5 / 100)
                        context.save();
                        context.translate(x, y);

                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                        } else {
                            context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                        }

                        context.restore();

                        var text1 = 'บริษัทกลางฯเท่านั้น';
                        context.translate(x, y);
                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                        } else {
                            context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                        }

                        return target;
                    })
                    .then((imgIdCard) => {
                        this.$store.state.inputUserData.base64IdCard = imgIdCard.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                    }).catch(function (error) {
                        alert(error);
                    });
            },
            stampWatermarksFaceImage(imgDataUrl) {
                var faceOptions = {
                    init(img) {
                        img.crossOrigin = 'anonymous';
                    }
                };
                watermark([imgDataUrl], faceOptions)
                    .dataUrl(function (target) {
                        var context = target.getContext('2d');
                        var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                        var x = (target.width / 2);
                        var y = (target.height / 2);
                        var fontSize = (target.width + target.height) * (3.5 / 100)
                        context.save();
                        context.translate(x, y);

                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                        } else {
                            context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                        }

                        context.restore();

                        var text1 = 'บริษัทกลางฯเท่านั้น';
                        context.translate(x, y);
                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                        } else {
                            context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                        }

                        return target;
                    })
                    .then((imgFace) => {
                        this.$store.state.inputUserData.base64Face = imgFace.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                    }).catch(function (error) {
                        alert(error);
                    });
                this.isLoading = false


            },

            stampWatermarksFromAccidentCreate() {
                //Stamp ลายน้ำ รูปสภาพแวดล้อมอุบัติเหตุ          
                for (let i = 0; i < this.$store.state.inputAccidentData.AccidentInput.accImages.length; i++) {
                    var optionsAcc = {
                        init(img) {
                            img.crossOrigin = 'anonymous';
                        }
                    };
                    var textRotateAcc = function (target) {
                        var context = target.getContext('2d');
                        var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                        var x = (target.width / 2);
                        var y = (target.height / 2);
                        var fontSize = (target.width + target.height) * (3.5 / 100)
                        context.save();
                        context.translate(x, y);

                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                        } else {
                            context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                        }

                        context.restore();

                        var text1 = 'บริษัทกลางฯเท่านั้น';
                        context.translate(x, y);
                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                        } else {
                            context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                        }

                        return target;
                    };
                    var resultAddWatermarkAcc = "";
                    watermark([this.$store.state.inputAccidentData.AccidentInput.accImages[i].base64], optionsAcc)
                        .image(textRotateAcc)
                        .then((imgBill) => {
                            resultAddWatermarkAcc = imgBill.src;
                            this.$store.state.inputAccidentData.AccidentInput.accImages[i].base64 = resultAddWatermarkAcc.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                        }).catch(function (error) {
                            alert(error);
                        });

                }
                //Stamp ลายน้ำ รูปสภาพรถที่เกิดอุบัติเหตุ 
                for (let i = 0; i < this.$store.state.inputAccidentData.AccidentCarInput.accCarImages.length; i++) {
                    var optionsAccCar = {
                        init(img) {
                            img.crossOrigin = 'anonymous';
                        }
                    };
                    var textRotateAccCar = function (target) {
                        var context = target.getContext('2d');
                        var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                        var x = (target.width / 2);
                        var y = (target.height / 2);
                        var fontSize = (target.width + target.height) * (3.5 / 100)
                        context.save();
                        context.translate(x, y);

                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                        } else {
                            context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                        }

                        context.restore();

                        var text1 = 'บริษัทกลางฯเท่านั้น';
                        context.translate(x, y);
                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                        } else {
                            context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                        }

                        return target;
                    };
                    var resultAddWatermarkAccCar = "";
                    watermark([this.$store.state.inputAccidentData.AccidentCarInput.accCarImages[i].base64], optionsAccCar)
                        .image(textRotateAccCar)
                        .then((imgBill) => {
                            resultAddWatermarkAccCar = imgBill.src;
                            this.$store.state.inputAccidentData.AccidentCarInput.accCarImages[i].base64 = resultAddWatermarkAccCar.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                        }).catch(function (error) {
                            alert(error);
                        });

                }
                //Stamp ลายน้ำ รูปสภาพอาการบาดเจ็บ
                for (let i = 0; i < this.$store.state.inputAccidentData.AccidentVictimInput.accVicBrokenImages.length; i++) {
                    var optionsAccVic = {
                        init(img) {
                            img.crossOrigin = 'anonymous';
                        }
                    };
                    var textRotateAccVic = function (target) {
                        var context = target.getContext('2d');
                        var text = 'ใช้สำหรับรับค่าสินไหมทดแทนจาก';
                        var x = (target.width / 2);
                        var y = (target.height / 2);
                        var fontSize = (target.width + target.height) * (3.5 / 100)
                        context.save();
                        context.translate(x, y);

                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text, ((x * 10) / 100) * 0.3, -((y * 10) / 100));

                        } else {
                            context.fillText(text, ((x * 10) / 100), -((y * 10) / 100));
                        }

                        context.restore();

                        var text1 = 'บริษัทกลางฯเท่านั้น';
                        context.translate(x, y);
                        if (target.width > target.height) {
                            context.rotate(-Math.atan((target.height - ((target.height * 20) / 100)) / target.width));
                        } else {
                            context.rotate(-Math.atan(target.height / target.width));
                        }
                        context.globalAlpha = 0.3;
                        context.fillStyle = 'red';
                        context.font = fontSize + 'px Kanit';
                        context.textAlign = 'center';
                        if (target.width > target.height) {
                            context.fillText(text1, -((x * 10) / 100) * 0.3, (y * 10) / 100);
                        } else {
                            context.fillText(text1, -((x * 10) / 100), (y * 10) / 100);
                        }

                        return target;
                    };
                    var resultAddWatermarkAccVic = "";
                    watermark([this.$store.state.inputAccidentData.AccidentVictimInput.accVicBrokenImages[i].base64], optionsAccVic)
                        .image(textRotateAccVic)
                        .then((imgBill) => {
                            resultAddWatermarkAccVic = imgBill.src;
                            this.$store.state.inputAccidentData.AccidentVictimInput.accVicBrokenImages[i].base64 = resultAddWatermarkAccVic.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
                        }).catch(function (error) {
                            alert(error);
                        });

                }
                this.isLoading = false
            },

        },
        mounted() {

        },
        async created() {
            await this.getJwtToken();
            if (this.$route.params.id != "Register") {
                this.accData = this.$store.getters.accGetter(this.$route.params.id)
                this.userData = this.$store.state.userStateData
            } else if (this.$route.params.id != "RegisterAccident") {
                this.userData = this.$store.state.userStateData
            }

            if (this.$route.params.from == "Create") {
                this.$store.state.inputApprovalData.SumMoney = parseFloat(this.$store.state.inputApprovalData.SumMoney)
                this.displayMaskTelNo = "xxx-xxx-" + this.userData.mobileNo.substr(this.userData.mobileNo.length - 4)
                this.fromText = "ส่งคำร้อง"
                this.stampWatermarksFromCreate()
            } else if (this.$route.params.from == "Edit") {
                this.$store.state.inputApprovalData.SumMoney = parseFloat(this.$store.state.inputApprovalData.SumMoney)
                this.displayMaskTelNo = "xxx-xxx-" + this.userData.mobileNo.substr(this.userData.mobileNo.length - 4)
                this.fromText = "ส่งเอกสารเพิ่มเติม"
                this.stampWatermarksFromEditApproval()
                this.isLoading = false
            } else if (this.$route.params.from == "CanselApproval") {
                this.displayMaskTelNo = "xxx-xxx-" + this.userData.mobileNo.substr(this.userData.mobileNo.length - 4)
                this.fromText = "ยกเลิกคำร้อง"
                this.isLoading = false
            } else if (this.$route.params.from == "CreateUser") {
                var telText = this.$store.state.inputUserData.mobileNo
                this.displayMaskTelNo = "xxx-xxx-" + telText.substr(telText.length - 4)
                this.fromText = "ลงทะเบียน"
                this.isLoading = true
                this.stampWatermarksIdCardImage(this.$store.state.inputUserData.base64IdCard)
                this.stampWatermarksFaceImage(this.$store.state.inputUserData.base64Face)
                this.isLoading = false
            } else if (this.$route.params.from == "ConfirmMoney") {
                this.displayMaskTelNo = "xxx-xxx-" + this.userData.mobileNo.substr(this.userData.mobileNo.length - 4)
                this.fromText = "ยอมรับจำนวนเงิน"
                this.isLoading = false
            } else if (this.$route.params.from == "AccidentCreate") {
                this.displayMaskTelNo = "xxx-xxx-" + this.userData.mobileNo.substr(this.userData.mobileNo.length - 4)
                this.fromText = "ส่งข้อมูลอุบัติเหตุเพื่อขอใช้สิทธิ์"
                this.stampWatermarksFromAccidentCreate()
            }
        }

    }
</script>
