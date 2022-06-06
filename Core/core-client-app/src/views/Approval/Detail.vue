<template>
    <div class="container" align="center">
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
        <h2 id="header2">รายละเอียดคำร้อง</h2>
        <div class="container">
            <div align="left" style="width: 100%;">
                <ion-icon name="people-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                <label align="left" class="title-advice-menu mb-1">ข้อมูลผู้ประสบภัย</label>
            </div>
            <div class="box-container mb-3">
                <div class="row">
                    <div class="col-9">
                        <p class="mb-0">ชื่อ-สกุล</p>
                        <div class="mt-0" v-if="approvalData.victim.prefix != null">
                            <p class="label-text">{{approvalData.victim.prefix}}{{approvalData.victim.fname}} {{approvalData.victim.lname}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.prefix === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-3">
                        <p class="mb-0">อายุ</p>
                        <div class="mt-0" v-if="approvalData.victim.age != null">
                            <p class="label-text">{{approvalData.victim.age}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.age === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-4">
                        <p class="mb-0">บ้านเลขที่</p>
                        <div class="mt-0" v-if="approvalData.victim.homeId != null">
                            <p class="label-text">{{approvalData.victim.homeId}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.homeId === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-2">
                        <p class="mb-0">หมู่</p>
                        <div class="mt-0" v-if="approvalData.victim.moo != null">
                            <p class="label-text">{{approvalData.victim.moo}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.moo === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">ซอย</p>
                        <div class="mt-0" v-if="approvalData.victim.soi != null">
                            <p class="label-text">{{approvalData.victim.soi}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.soi === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">ถนน</p>
                        <div class="mt-0" v-if="approvalData.victim.road != null">
                            <p class="label-text">{{approvalData.victim.road}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.road === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">ตำบล/แขวง</p>
                        <div class="mt-0" v-if="approvalData.victim.tumbolName != null">
                            <p class="label-text">{{approvalData.victim.tumbolName}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.tumbolName === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">อำเภอ</p>
                        <div class="mt-0" v-if="approvalData.victim.districtName != null">
                            <p class="label-text">{{approvalData.victim.districtName}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.districtName === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">จังหวัด</p>
                        <div class="mt-0" v-if="approvalData.victim.provinceName != null">
                            <p class="label-text">{{approvalData.victim.provinceName}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.provinceName === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">รหัสไปรษณีย์</p>
                        <div class="mt-0" v-if="approvalData.victim.zipcode != null">
                            <p class="label-text">{{approvalData.victim.zipcode}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.zipcode === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">เบอร์โทรศัพท์</p>
                        <div class="mt-0" v-if="approvalData.victim.telNo != null">
                            <p class="label-text">{{approvalData.victim.telNo}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="approvalData.victim.telNo === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
            </div>

            <!-- เหตุ -->
            <div align="left" style="width: 100%;">
                <ion-icon name="bicycle-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                <label align="left" class="title-advice-menu mb-1">ข้อมูลอุบัติเหตุ</label>
            </div>
            <div class="box-container mb-3">

                <p class="mb-0">วันที่เกิดเหตุ</p>
                <div class="mt-0" v-if="accData.stringAccDate != null">
                    <p class="label-text">{{accData.stringAccDate}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accData.stringAccDate === null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">ลักษณะเกิดเหตุ</p>
                <div class="mt-0" v-if="accData.accNature != null">
                    <p class="label-text">{{accData.accNature}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accData.accNature === null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">สถานที่เกิดเหตุ</p>
                <div class="mt-0" v-if="accData.placeAcc != null">
                    <p class="label-text">{{accData.placeAcc}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accData.placeAcc === null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">จังหวัดที่เกิดเหตุ</p>
                <div class="mt-0" v-if="accData.provAcc != null">
                    <p class="label-text">{{accData.provAcc}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accData.provAcc === null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">หมายเลขทะเบียนรถคันเอาประกันภัย</p>
                <div class="mt-0" v-if="approvalData.car.foundCarLicense != null">
                    <p class="label-text">{{approvalData.car.foundCarLicense}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="approvalData.car.foundCarLicense === null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">เลขตัวถัง</p>
                <div class="mt-0" v-if="approvalData.car.foundChassisNo != null">
                    <p class="label-text">{{approvalData.car.foundChassisNo}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="approvalData.car.foundChassisNo === null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">กรมธรรม์คุ้มครองภัยจากรถ เลขที่</p>
                <div class="mt-0" v-if="approvalData.car.foundPolicyNo != null">
                    <p class="label-text">{{approvalData.car.foundPolicyNo}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="approvalData.car.foundPolicyNo === null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>

            </div>
            <!-- บัญชี -->
            <div align="left" style="width: 100%;">
                <ion-icon name="card-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                <label align="left" class="title-advice-menu mb-1">ข้อมูลบัญชีรับเงิน</label>
            </div>
            <div class="box-container mb-3">
                <div>
                    <p class="mb-0">หน้าสมุดบัญชีธนาคาร</p>

                    <div align="center">
                        <div class="div-center-image" @click="showBigImage(approvalData.bankAccount.base64Image)">
                            <div class="divImage">
                                <img class="img-show" :src="approvalData.bankAccount.base64Image" />
                                <br />
                            </div>
                        </div>

                    </div>

                    <p class="mb-0">ชื่อธนาคาร</p>
                    <div class="mt-0" v-if="approvalData.bankAccount.accountBankName != null">
                        <p class="label-text">{{ approvalData.bankAccount.accountBankName }}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="approvalData.bankAccount.accountBankName === null">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>

                    <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                    <div class="mt-0" v-if="approvalData.bankAccount.accountName != null">
                        <p class="label-text">{{approvalData.bankAccount.accountName}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="approvalData.bankAccount.accountName === null">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">เลขบัญชีธนาคาร</p>
                    <div class="mt-0" v-if="approvalData.bankAccount.accountNumber != null">
                        <p class="label-text">{{approvalData.bankAccount.accountNumber}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="approvalData.bankAccount.accountNumber === null">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                </div>




            </div>
            <!-- เอกสาร -->
            <div align="left" style="width: 100%;">
                <ion-icon name="reader-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                <label align="left" class="title-advice-menu mb-1">ข้อมูลเอกสารประกอบคำร้อง</label>
            </div>
            <div class="box-container mb-3">
                <div class="card-bill" v-for="invhd in approvalData.invoicehds" :key="invhd.idinvhd">
                    <p class="mb-2">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                    <div class="div-center-image" v-for="(input, index) in invhd.base64Image" :key="index" @click="showBigImage(invhd.base64Image[index])">
                        <div class="divImage" v-if="invhd.base64Image[index] != null" align="center">
                            <img class="img-show" :src="invhd.base64Image[index]" />
                            <br />
                        </div>
                    </div>

                    <p class="mb-0">อาการบาดเจ็บ</p>
                    <div class="mt-0" v-if="invhd.woundedName != null">
                        <p class="label-text">{{invhd.woundedName}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="invhd.woundedName === null">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">ประเภทผู้ป่วย</p>
                    <div class="mt-0" v-if="invhd.victimType != null">
                        <p class="label-text">{{invhd.victimType}}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="invhd.victimType === null">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <p class="mb-0">ชื่อโรงพยาบาล</p>
                    <div class="mt-0" v-if="invhd.hospitalName != null">
                        <p class="label-text">{{ invhd.hospitalName }}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="invhd.hospitalName === null">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <div class="row">
                        <div class="col-7">
                            <p class="mb-0">วันที่เข้ารักษา</p>
                            <div class="mt-0" v-if="invhd.stringTakendate != null">
                                <p class="label-text">{{invhd.stringTakendate}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="invhd.stringTakendate === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-5">
                            <p class="mb-0">เวลา</p>
                            <div class="mt-0" v-if="invhd.takentime != null">
                                <p class="label-text">{{invhd.takentime}} น.</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="invhd.takentime === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-7">
                            <p class="mb-0">วันที่ออกจากโรงพยาบาล</p>
                            <div class="mt-0" v-if="invhd.stringDispensedate != null">
                                <p class="label-text">{{invhd.stringDispensedate}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="invhd.stringDispensedate === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-5">
                            <p class="mb-0">เวลา</p>
                            <div class="mt-0" v-if="invhd.dispensetime != null">
                                <p class="label-text">{{invhd.dispensetime}} น.</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="invhd.dispensetime === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-7">
                            <p class="mb-0">เลขที่ใบเสร็จ</p>
                            <div class="mt-0" v-if="invhd.receiptNo != null">
                                <p class="label-text">{{invhd.receiptNo}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="invhd.receiptNo === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-5">
                            <p class="mb-0">จำนวนเงิน</p>
                            <div class="mt-0" v-if="invhd.suminv != null">
                                <p class="label-text">{{invhd.suminv}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="invhd.suminv === null">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>

                </div>
                <p class="mb-0">จำนวนเงินรวมทั้งหมด</p>
                <div class="mt-0" v-if="approvalData.sumReqMoney > 0">
                    <p class="label-text">{{approvalData.sumReqMoney}} บาท</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="approvalData.sumReqMoney <= 0">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
            </div>


        </div>
    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'
    import axios from 'axios'
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: "ClaimDetail",
        mixins: [mixin],
        components: {
            Loading
        },
        data() {
            return {
                bank: null,
                total_amount: null,
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                //----Get ApprovalData
                approvalData: {
                    car: {
                        foundCarLicense: null,
                        foundChassisNo: null,
                        foundPolicyNo: null
                    },
                    victim: {
                        accNo: null, victimNo: null, prefix: null, fname: null, lname: null, sex: null, age: null,
                        drvSocNo: null, homeId: null, moo: null, soi: null, road: null, tumbol: null, tumbolName: null,
                        district: null, districtName: null, province: null, provinceName: null, zipcode: null, telNo: null
                    },
                    bankAccount: {
                        accountBankName: null,
                        accountName: null,
                        accountNumber: null,
                        bankId: null,
                        base64Image: null

                    }
                },
                isLoading: true,
                modalBigImage: false,
                srcBigImage: null,



            }
        },
        methods: {
            getApprovalDetail() {
                var url = this.$store.state.envUrl + '/api/Approval/ApprovalDetail';
                const body = {
                    AccNo: this.$route.params.id,
                    VictimNo: parseInt(this.accData.victimNo),
                    ReqNo: parseInt(this.$route.params.appNo),
                    UserIdCard: this.userData.idcardNo
                };
                var apiConfig = {
                    headers: {
                        'Authorization': "Bearer " + this.$store.state.jwtToken.token,
                        'Content-Type': 'application/json',
                    }
                }
                axios.post(url, JSON.stringify(body), apiConfig)
                    .then((response) => {
                        this.approvalData = response.data;
                        this.getBankFileFromECM();
                        for (let i = 0; i < this.approvalData.invoicehds.length; i++) {
                            for (let j = 0; j < this.approvalData.invoicehds[i].base64Image.length; j++) {
                                this.approvalData.invoicehds[i].base64Image[j] = 'data:image/png;base64,' + this.approvalData.invoicehds[i].base64Image[j]

                            }
                        }

                    })
                    .catch((error) => {
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.getApprovalDetail()
                        }
                    });
            },
            getBankFileFromECM() {

                this.downloadFileFromECM(process.env.VUE_APP_API_ECM_DOWNLOAD_BANK_ACCOUNT_FILE_SYSTEM_ID, process.env.VUE_APP_API_ECM_DOWNLOAD_BANK_ACCOUNT_FILE_TEMPLATE_ID, process.env.VUE_APP_API_ECM_DOWNLOAD_BANK_ACCOUNT_FILE_DOCUMENT_ID, this.$route.params.appNo + '|' + this.accData.accNo + '|' + this.accData.victimNo)
                    .then((response) => {
                        this.approvalData.bankAccount.base64Image = 'data:image/png;base64,' + response.data;
                        this.isLoading = false;
                    }).catch((error) => {
                        alert(error)
                    });

            },
            showBigImage(src) {
                this.modalBigImage = true
                this.srcBigImage = src

            }
        },
        async mounted() {
            this.getJwtToken();
            await this.getApprovalDetail();


        },
    }
</script>
<style>
    .label-text {
        margin-bottom: 0px;
        color: gray;
    }
</style>