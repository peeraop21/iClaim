<template>
    <div class="container space-contianer" align="center">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>
        <vs-dialog width="550px" not-center v-model="modalBigImage">
            <div class="con-content" align="center">
                <img class="big-img-show" :src="srcBigImage" />
            </div>
        </vs-dialog>
        <div class="container">
            <div class="row">
                <div class="col-12" align="center">
                    <h2 id="header2">ยืนยันจำนวนเงิน</h2>
                </div>
                <div class="col-12" align="center">
                    <div class="tab-user mt-4 mb-4">
                        <div class="col-12">
                            <div class="title-claim-number" style="margin-top: 0px; margin-bottom: 0px; line-height: 4px">
                                <p v-if="accData.accNo!=''">เลขที่รับแจ้ง : {{accData.accNo}}</p>
                                <p v-else-if="accData.accNo===''">เลขที่รับแจ้ง : -</p>
                                <p v-if="accData.lastClaim.claimNo!=''">เลขที่เคลม : {{accData.lastClaim.claimNo}}</p>
                                <p v-else-if="accData.lastClaim.claimNo===''">เลขที่เคลม : -</p>
                                <p v-if="confirmMoneyData.car.foundPolicyNo != null">เลขที่กรมธรรม์ : {{confirmMoneyData.car.foundPolicyNo}}</p>
                                <p v-else-if="confirmMoneyData.car.foundPolicyNo === null">เลขที่กรมธรรม์ : -</p>
                                <p v-if="confirmMoneyData.reqDate != null">วันที่ยื่นคำร้อง : {{confirmMoneyData.reqDate}} เวลา {{confirmMoneyData.reqTime}} น.</p>
                                <p v-else-if="confirmMoneyData.reqDate === null">วันที่ยื่นคำร้อง : -</p>
                                <p v-if="hosData.appNo!=''" style="margin-bottom: 3px">คำร้องขอที่ : {{hosData.appNo}}</p>
                                <p v-else-if="hosData.appNo===''" style="margin-bottom: 3px">คำร้องขอที่ : -</p>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="col-12">
                    <div class="text-start">
                        <div>
                            <span class="fw-bold">&emsp;&emsp;ตามที่ท่านได้ทำการยื่นคำร้องขอเบิกค่ารักษาพยาบาลเบื้องต้น มีบางรายการที่ไม่สามารถจ่ายได้ เนื่องจากไม่เกี่ยวกับค่ารักษาพยาบาลจำเป็นต้องตัดออก และมีบางรายการจำเป็นต้องหักออก เนื่องจากเกินราคามาตรฐานกลางฯ</span>
                            <!--<span class="p-main-color">{{confirmMoneyData.sumReqMoney}}</span>มีบางรายการที่ไม่สามารถจ่ายได้ เนื่องจากไม่เกี่ยวกับค่ารักษาพยาบาลจำเป็นต้องตัดออก และมีบางรายการจำเป็นต้องหักออก เนื่องจากเกินราคามาตรฐานกลาง-->
                        </div>
                        <div class="my-3" v-for="inv in confirmMoneyData.invoiceList" :key="inv.idInvhd">
                            <div class="div-center-image">
                                <div class="divImage" style="width: 100%;border-style: solid;" v-if="inv.base64Image != null" align="left">
                                    <div class="row">
                                        <div class="col-5" align="center" @click="showBigImage(inv.base64Image)">
                                            <img class="img-show" :src="inv.base64Image" />
                                        </div>
                                        <div class="col-7 px-0">
                                            <div class="row mt-2">
                                                <p class="inv-text">เล่มที่ใบเสร็จ : {{inv.bookNo}}</p>
                                                <p class="inv-text">เลขที่ใบเสร็จ : {{inv.receiptNo}}</p>
                                                <p class="inv-text" style="color:red">จำนวนเงินที่ร้องขอ : {{inv.reqMoney}} บาท</p>
                                                <p class="inv-text" style="color:green">จำนวนเงินที่จ่ายได้ : {{inv.payMoney}} บาท</p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12" style="width: 95%; margin-inline: auto;">
                                        <table id="treatments" class=" mb-4 mt-2" style="font-size:12px">
                                            <thead>
                                                <tr>
                                                    <th>รายการที่สามารถจ่ายได้</th>
                                                    <th>จำนวนเงิน</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr v-for="item in inv.invoicedtVerify.items" :key="item.listNo" >
                                                    <td>
                                                        <p class="mb-0">{{item.treatName}}</p>
                                                        <p class="mb-0" v-if="item.reqMoney != item.paidMoney" style="color:red; font-size:10px">*มีการหักรายการนี้ออก {{item.reqMoney - item.paidMoney}} บาท</p>
                                                    </td>
                                                    <td align="center" style="width:30%">{{item.paidMoney}} บาท</td>

                                                </tr>

                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="col-12" style="width: 95%; margin-inline: auto;">
                                        <table  id="treatments-danger" class="mb-4 mt-2" style="font-size:12px">
                                            <thead>
                                                <tr>
                                                    <th>รายการที่ถูกตัดออก (ไม่ใช่ค่ารักษา)</th>
                                                    <th>จำนวนเงิน</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr v-for="item in inv.invoiceCutList" :key="item.listNo" >
                                                    <td>
                                                        {{item.cutListName}}
                                                    </td>
                                                    <td align="center" style="width:30%">{{item.cutListPrice}} บาท</td>

                                                </tr>

                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div v-if="confirmMoneyData.sumReqMoney != null">
                            &emsp;&emsp;เอกสารฉบับนี้เป็นเอกสารยืนยันการรับเงินค่ารักษาพยาบาลเบื้องต้น ตามเงื่อนไขกรมธรรม์คุ้มครองผู้ประสบภัยจากรถ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด เป็นจำนวนเงิน
                            <span class="p-main-color">{{confirmMoneyData.sumPayMoney}}</span> บาท

                            และ {{userData.prefix}}{{userData.fname}} {{userData.lname}} มีความประสงค์รับเงินดังกล่าว
                        </div>
                    </div>
                    <div class="text-start mt-4 mb-4">
                        <div>
                            <p v-if="confirmMoneyData.bankAccount.accountBankName != null">
                                โดยการโอนเงินเข้าบัญชีธนาคาร
                                <span class="p-main-color"> {{ confirmMoneyData.bankAccount.accountBankName }}</span>
                            </p>
                            <p v-else-if="confirmMoneyData.bankAccount.accountBankName === null">โดยการโอนเงินเข้าบัญชีธนาคาร -</p>
                        </div>
                        <div style="margin-top: -15px">
                            <p v-if="confirmMoneyData.bankAccount.accountName != null">
                                ชื่อบัญชีธนาคาร
                                <span class="p-main-color"> {{ confirmMoneyData.bankAccount.accountName }}</span>
                            </p>
                            <p v-else-if="confirmMoneyData.bankAccount.accountName === null">ชื่อบัญชีธนาคาร -</p>
                        </div>
                        <div style="margin-top: -15px">
                            <p v-if="confirmMoneyData.bankAccount.accountNumber != null">
                                เลขที่บัญชีธนาคาร
                                <span class="p-main-color"> {{ confirmMoneyData.bankAccount.accountNumber }}</span>
                            </p>
                            <p v-else-if="confirmMoneyData.bankAccount.accountNumber === null">เลขที่บัญชีธนาคาร -</p>
                        </div>
                    </div>
                </div>


            </div>
            <div class="row mb-2">
                <div class="col-2" style="padding-right: 0px; width:13%;">
                    <vs-checkbox v-model="acceptR" color="var(--main-color)" @change="tik">
                        <template #icon>
                            <i class='ti ti-check'></i>
                        </template>
                    </vs-checkbox>
                </div>
                <div class="col-10 px-0">
                    <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                        ข้าพเจ้าขอรับรองว่าข้อความทั้งหมดนี้มีความถูกต้องและเป็นความจริง ซึ่งเป็นไปตามความประสงค์ของข้าพเจ้า และเมื่อได้รับการโอนเงินเข้าบัญชีดังกล่าวภายใน 7 วัน นับจากวันที่ลงนามนี้แล้ว ให้ถือว่าข้าพเจ้าได้รับค่าเสียหายเบื้องต้น จากบริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด ครบถ้วนสมบูรณ์ทุกประการ
                    </p>
                </div>
            </div>
            <div class="row" v-if="acceptR">
                <br>
                <button class="btn-confirm-money" type="button" @click="submit">{{lblButton}}</button>

            </div>
        </div>

    </div>
</template>

<style>
    #treatments-danger {
        border-collapse: collapse;
        width: 100%;
        border-radius: 20px;
    }

        #treatments-danger td, #treatments-danger th {
            border: 1px solid #ccc;
            padding: 8px;
        }

        #treatments-danger tr:nth-child(even) {
            background-color: #ddd;
        }

        #treatments-danger tr:hover {
            background-color: white;
        }

        #treatments-danger th {
            border: 1.8px solid #ddd;
            padding-top: 12px;
            padding-bottom: 12px;
            text-align: center;
            background-color: indianred;
            color: white;
        }

    .inv-text {
        padding: 0px;
        margin-bottom: 2px;
        font-size: 12px;
    }

    .btn-confirm-money {
        background-color: var(--main-color);
        border: none;
        color: white;
        padding: 5px 20px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }

    .title-claim-number {
        font-weight: bold;
        font-size: 12px;
        text-align: start;
    }

    .p-main-color {
        color: var(--main-color);
        font-weight: bold;
        font-size: 13px;
        text-align: start;
    }
</style>

<script>
    import mixin from '../../mixin/index.js'
    import axios from 'axios'
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: "ConfirmMoney",
        mixins: [mixin],
        components: {
            Loading
        },
        data() {
            return {
                acceptR: false,
                lblButton: 'ยืนยันจำนวนเงิน',
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                bank: null,
                total_amount: null,

                // HosApp
                hosData: this.$store.getters.hosAppGetter(this.$route.params.id, this.$route.params.appNo),
                countDown: 60,
                disableBtnReqOTP: false,
                inputOTP: "",
                confirmMoneyData: {
                    reqNo: null, reqDate: null, reqTime: null, sumReqMoney: null, sumPayMoney: null,
                    car: {
                        foundCarLicense: null, foundChassisNo: null, foundPolicyNo: null
                    },
                    bankAccount: {
                        accountBankName: null,
                        accountName: null,
                        accountNumber: null,
                        bankId: null
                    },
                    invoiceList: [{
                        idInvhd: null,
                        bookNo: null,
                        receiptNo: null,
                        reqMoney: null,
                        payMoney: null

                    }]
                },
                bankNames: null,
                isLoading: true,
                modalBigImage: false,
                srcBigImage: null,
            }
        },
        methods: {
            showBigImage(src) {
                this.modalBigImage = true
                this.srcBigImage = src
            },
            tik() {
                console.log(this.acceptR)
            },
            showSwal() {
                this.$swal({
                    icon: 'success',
                    text: 'บริษัทจะแจ้งวันที่โอนเงินให้ท่านทราบอีกครั้ง',
                    title: 'ยืนยันจำนวนเงินเรียบร้อย',
                    showCancelButton: false,
                    showDenyButton: false,
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                    }
                })
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

            //getBankNames() {
            //    var url = this.$store.state.envUrl + '/api/Master/Bank';
            //    axios.get(url)
            //        .then((response) => {
            //            this.bankNames = response.data;
            //            this.getDataConfirmMoney();
            //        })
            //        .catch(function (error) {
            //            alert(error);
            //        });
            //},
            getDataConfirmMoney() {
                var url = this.$store.state.envUrl + '/api/Approval/DataConfirmMoney';
                const body = {
                    AccNo: this.$route.params.id,
                    VictimNo: parseInt(this.accData.victimNo),
                    ReqNo: parseInt(this.$route.params.appNo),
                };
                var apiConfig = {
                    headers: {
                        'Authorization': "Bearer " + this.$store.state.jwtToken.token,
                        'Content-Type': 'application/json',
                    }
                }
                axios.post(url, JSON.stringify(body), apiConfig)
                    .then((response) => {
                        this.bankNames = response.data.banks;
                        this.confirmMoneyData = response.data.confirmMoneyData;
                        console.log(this.confirmMoneyData)
                        if (this.confirmMoneyData != null) {
                            for (let i = 0; i < this.bankNames.length; i++) {
                                if (this.confirmMoneyData.bankAccount.accountBankName == this.bankNames[i].bankCode) {
                                    this.confirmMoneyData.bankAccount.accountBankName = this.bankNames[i].name
                                    this.confirmMoneyData.bankAccount.bankId = this.bankNames[i].bankCode

                                }
                            }
                        }
                        for (let i = 0; i < this.confirmMoneyData.invoiceList.length; i++) {
                            this.confirmMoneyData.invoiceList[i].base64Image = 'data:image/png;base64,' + this.confirmMoneyData.invoiceList[i].base64Image
                        }
                        this.isLoading = false;
                    })
                    .catch((error) => {
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.getDataConfirmMoney()
                        }
                    });
            },
            countDownTimer() {
                if (this.countDown > 0) {
                    this.disableBtnReqOTP = true
                    setTimeout(() => {
                        this.countDown -= 1
                        this.countDownTimer()
                    }, 1000)
                } else if (this.countDown <= 0) {
                    this.disableBtnReqOTP = false
                    this.countDown = 60

                }
            },
            submit() {
                this.$swal({
                    icon: 'question',
                    text: 'ท่านยืนยันจะรับจำนวนเงินในครั้งนี้หรือไม่?',
                    /*title: 'แจ้งเตือน',*/
                    /*footer: '<a href="">Why do I have this issue?</a>'*/
                    showCancelButton: false,
                    showDenyButton: true,
                    denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยกเลิก",
                    denyButtonColor: '#dad5e9',
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยืนยัน",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {

                    }
                }).then((result) => {

                    if (result.isConfirmed) {
                        this.$router.push({ name: 'ConfirmOTP', params: { from: "ConfirmMoney", accNo: this.$route.params.id, victimNo: this.accData.victimNo, appNo: this.$route.params.appNo } })
                    }
                    //} else if (result.isDenied) {

                    //}
                });
            },
            



            showSwalError() {
                this.$swal({
                    icon: 'error',
                    text: 'เนื่องจากสถานะคำร้องของท่านไม่ได้อยู่ระหว่างการยืนยันจำนวนเงิน',
                    title: 'ไม่สามารถยืนยันจำนวนเงินได้',
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
                        this.$router.push({ name: 'Accident' })
                    }
                });
            },
            showSwalSuccess() {
                this.$swal({
                    icon: 'success',
                    text: 'บริษัทจะแจ้งวันที่โอนเงินให้ท่านทราบอีกครั้ง',
                    title: 'ยืนยันจำนวนเงินเรียบร้อย',
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
                        this.$router.push({ name: 'Accident' })
                    }
                });
            },
        },
        async created() {
            await this.getDataConfirmMoney();
        }



    };
</script>
