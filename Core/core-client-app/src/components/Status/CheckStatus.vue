<template>
    <div class="container">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">ประวัติ/สถานะคำร้อง</h2>

                <!--<div class="wrap-collabsible">
                    <input id="collapsible" class="toggle" type="checkbox">
                    <label for="collapsible" class="lbl-toggle">More Info</label>
                    <div class="collapsible-content">
                        <div class="content-inner">
                            <p>
                                QUnit is by calling one of the object that are embedded in JavaScript, and faster JavaScript program could also used with its elegant, well documented, and functional programming using JS, HTML pages Modernizr is a popular browsers without plug-ins. Test-Driven Development.
                            </p>
                        </div>
                    </div>
                </div>-->
                <section>
                    <div v-if="isHasIclaimApprovalData == true" style="height: 90%; width: 100%;">
                        <div class="accordion" v-for="iclaimApp in iclaimApprovalData" :key="iclaimApp.appNo">
                            <div class="accordion-item" :id="'list' + iclaimApp.appNo">
                                <a class="accordion-link" :href="'#list' + iclaimApp.appNo">
                                    <div>
                                        <p style="margin-bottom: 10px">
                                            <ion-icon name="newspaper-outline"></ion-icon>คำร้องที่: {{ iclaimApp.appNo }}
                                            <br>
                                            <ion-icon name="calendar-outline"></ion-icon>วันที่ยื่นคำร้อง: {{ iclaimApp.stringRegDate }} น.
                                            <br>
                                            <ion-icon name="options-outline"></ion-icon>สถานะคำร้อง: {{iclaimApp.appStatusName}}
                                        </p>
                                    </div>
                                    <div align="right" style="margin-top: -10px;">
                                        <div style="margin-top:-5px">
                                            <!--<a v-on:click="getPDF(iclaimApp.appNo)">PDF</a>-->
                                            <vs-button v-on:click="externalPagePDF(iclaimApp.appNo)"
                                                       icon
                                                       primary
                                                       flat>
                                                PDF
                                            </vs-button>
                                            <router-link :to="{ name: 'ClaimDetail', params: { id: iclaimApp.stringAccNo, appNo: iclaimApp.appNo}}">
                                                <vs-button circle
                                                           icon
                                                           primary
                                                           flat>
                                                    <ion-icon name="information" style="margin: -2px -7px -2px 0px; font-size: 17px"></ion-icon>
                                                </vs-button>
                                            </router-link>
                                        </div>
                                        <ion-icon name="chevron-down-outline" class="icon ion-md-add" style="margin-right: 6px; margin-top: 10px"></ion-icon>
                                    </div>
                                </a>
                                <div class="answer" style="padding-left:30px;">
                                    
                                    <p class="p-custom custom-p-status">สถานะ </p>

                                    <ul id="progress" v-for="status in iclaimApp.appStatus" :key="status.statusId">
                                        <li class="li-custom " :id="'liLbl' + status.statusId">
                                            <p v-if="status.statusDate != null" class="p-custom divider-p" style="font-size: 8px; position: absolute; left: -12px; margin-top: -1px;">{{status.statusDate}}</p>
                                            <p v-if="status.statusTime != null" class="p-custom divider-p" style="font-size: 10px; position: absolute; left: -1px; margin-top: 7px; ">{{status.statusTime}}</p>
                                            <div class="node " v-bind:class="{green:status.active, grey:!status.active}"></div>
                                            <p class="p-custom divider-p" v-bind:class="{pgreen:status.active}">{{status.statusName}}</p>
                                        </li>
                                        <li v-if="status.statusId < iclaimApp.appStatus.length + iclaimApp.appStatus.length" class="li-custom " :id="'verticalLine' + status.statusId">
                                            <div class="divider grey"></div>
                                        </li>

                                    </ul>
                                    <br />
                                </div>
                                <div style="text-align: center">
                                    <router-link class="btn-select" v-if="iclaimApp.status == 2" :to="{ name: 'AddDocument', params: { id: iclaimApp.stringAccNo, appNo: iclaimApp.appNo }}" style="padding-right: 10px; padding-left: 10px">แนบเอกสารเพิ่มเติม</router-link>
                                    <router-link class="btn-checked" v-if="iclaimApp.appStatusName == 'รอยืนยันจำนวนเงิน'" :to="{ name: 'ConfirmMoney', params: { id: iclaimApp.stringAccNo, appNo: iclaimApp.appNo}}">ยอมรับจำนวนเงิน</router-link>
                                    <router-link class="btn-checked" v-if="iclaimApp.appStatusName == 'โอนเงิน'" :to="{ name: 'Rating', params: { id: iclaimApp.stringAccNo }}">ประเมินความพึงพอใจ</router-link>
                                </div>
                                <div style="text-align: center">
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                    <p v-if="isHasIclaimApprovalData == false" class="notData">- ไม่มีคำร้อง -</p>
                </section>
            </div>
        </div>
        <br>
    </div>
</template>

<style>
    .verticalLine {
        border-left: 1px solid #bbb;
        margin-left: 25px;
    }

    *, *:after, *:before {
        margin: 0;
        padding: 0;
    }

    #progress {
        margin-bottom: 0rem;
        height: 24px;
    }

    .node {
        height: 10px;
        width: 10px;
        border-radius: 50%;
        display: inline-block;
        transition: all 1000ms ease;
    }

    .answer .p-custom {
        color: none;
        padding: 0rem 0 0 2rem;
    }

    .activated {
        box-shadow: 0px 0px 3px 2px rgba(194, 255, 194, 0.8);
    }

    .pgreen {
        color: rgba(92, 184, 92, 1);
    }

    .divider {
        height: 20px;
        width: 2px;
        margin-left: 4px;
        transition: all 800ms ease;
    }

    .divider-p {
        display: inline-block;
        margin-left: -10px;
        margin-bottom: 0px;
    }

    .li-custom {
        list-style: none;
        line-height: 1px;
    }

    .custom-p-status {
        color: black;
        margin-bottom: 5px;
        margin-top: 5px;
    }

    .blue {
        background-color: rgba(82, 165, 255, 1);
    }

    .green {
        background-color: rgba(92, 184, 92, 1);
    }

    .red {
        background-color: rgba(255, 148, 148, 1);
    }

    .grey {
        background-color: rgba(201, 201, 201, 1);
    }
</style>
<script>
    import liff from '@line/liff'
    import axios from 'axios'
    export default {
        name: 'Accident',
        data() {
            return {
                accidents: [
                    {
                        id: 1,
                        accident_no: "64/XXX/00001",
                        accident_date: "05/06/2564",
                        status: "รับคำร้อง",
                        money: 10000
                    },
                    {
                        id: 2,
                        accident_no: "64/XXX/00002",
                        accident_date: "29/07/2564",
                        status: "ตรวจสอบคำร้อง",
                        money: 4000
                    },
                    {
                        id: 3,
                        accident_no: "64/XXX/00003",
                        accident_date: "13/08/2564",
                        license_plare: "พิจารณาจ่าย",
                        money: 7000
                    }
                ],

                accData: this.$store.getters.accGetter(this.$route.params.id),
                iclaimApprovalData: null,
                isHasIclaimApprovalData: false,
                appStatus: [{ statusId: 0, statusName: "", active: false, statusDate: "", statusTime: "" }],
                isActive: true,
                pdfsrc: null


            }

        },
        methods: {

            getIclaimApproval() {
                var url = '/api/approval/HosApproval/{accNo}/{victimNo}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.victimNo);

                axios.get(url)
                    .then((response) => {
                        /*this.userApi = response.data;*/
                        this.$store.state.hosAppStateData = response.data;
                        this.iclaimApprovalData = this.$store.state.hosAppStateData;
                        var appNoDocumentNotPass = [];
                        var appNoConfirmMoney = [];
                        var showNoti = false;
                        for (let i = 0; i < this.iclaimApprovalData.length; i++) {
                            if (this.iclaimApprovalData[i].status == 2 ) {
                                appNoDocumentNotPass.push(this.iclaimApprovalData[i].appNo);
                                showNoti = true;
                            }
                            if (this.iclaimApprovalData[i].status == 4) {
                                appNoConfirmMoney.push(this.iclaimApprovalData[i].appNo);
                                showNoti = true;
                            }
                        }
                        if (showNoti) {
                            var htmlMessage = "";
                            htmlMessage = htmlMessage + '<p align="left"> <strong>คำร้องที่ : ' + appNoDocumentNotPass + ' </strong><br>&emsp;&emsp;เอกสารของท่านไม่สมบูรณ์ กรุณากดที่ปุ่ม <label style="color:var(--main-color)">"แนบเอกสารเพิ่มเติม"</label> เพื่อดูรายละเอียด และแนบเอกสารที่ไม่สมบูรณ์</p>'
                            if (appNoConfirmMoney.length > 0) {
                                htmlMessage = htmlMessage + '<p align="left"> <strong>คำร้องที่ : ' + appNoConfirmMoney + ' </strong><br>&emsp;&emsp;เนื่องจากจำนวนเงินที่เจ้าหน้าที่พิจารณา ไม่ตรงตามจำนวนเงินที่ท่านส่งคำร้องขอไป กรุณากดที่ปุ่ม <label style="color:var(--main-color)">"ยอมรับจำนวนเงิน"</label> เพื่อดูรายละเอียด และยอมรับจำนวนเงิน</p>'
                                
                            }
                            
                            this.$swal({
                                icon: 'info',
                                html: htmlMessage,
                                title: 'แจ้งเตือน',
                                /*footer: '<a href="">Why do I have this issue?</a>'*/
                                showCancelButton: false,
                                showDenyButton: false,

                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {

                                }
                            })
                        }
                        
                        console.log("hosApp: ", this.iclaimApprovalData.length);
                        console.log("hosApp: ", this.iclaimApprovalData);
                        if (this.iclaimApprovalData.length == 0) {
                            this.isHasIclaimApprovalData = false
                        } else if (this.iclaimApprovalData.length > 0) {
                            this.isHasIclaimApprovalData = true
                        }
                    })
                    .catch(function (error) {
                        alert(error);
                    });

            },
            externalPagePDF(appNo) {
                liff.openWindow({
                    url: 'https://ts2digitalclaim.rvp.co.th/DownloadPDF/{accNo}/{victimNo}/{appNo}/{channel}/?openExternalBrowser=1'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', appNo).replace('{channel}', this.accData.channel)
                });
            },
            getPDF(appNo) {

                /*var url = '/api/genpdf/GetBoto3/{accNo}/{victimNo}/{appNo}/{channel}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', appNo).replace('{channel}', this.accData.channel);*/
                var url = '/api/genpdf';
                const body = {
                    AccNo: this.$route.params.id,
                    VictimNo: this.accData.victimNo,
                    AppNo: appNo,
                    Channel: this.accData.channel
                };
                this.$swal({
                    title: 'กำลังโหลด',
                    html: 'ขณะนี้ระบบกำลังโหลดเอกสาร คำร้องขอรับค่าเสียหายเบื้องต้น',
                    timerProgressBar: true,
                    didOpen: () => {
                        this.$swal.showLoading()

                    },
                    willClose: () => {

                    }
                })
                axios.post(url, JSON.stringify(body), {
                    responseType: 'arraybuffer',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                })
                    .then((response) => {
                        console.log(response)
                        let blob = new Blob([response.data], { type: 'application/pdf' }),
                            url = window.URL.createObjectURL(blob)
                        this.$swal.close();
                        //this.$router.push(url)
                        //window.location.href = url;
                        console.log(url)
                        /*window.open(url)*/
                        liff.openWindow({
                            url: 'https://www.youtube.com/?openExternalBrowser=1'
                        });
                        
                        /*window.open("data:application/pdf," + encodeURI(response.data));*/
                        //const blob = new Blob([response.data]);
                        //const objectUrl = URL.createObjectURL(blob);
                        //this.pdfsrc = objectUrl;
                        //console.log(this.pdfsrc)

                        //var link = document.createElement('a');
                        //link.href = window.URL.createObjectURL(blob);
                        //var fileName = "PT3";
                        //link.download = fileName;
                        //link.click();
                        //this.$swal.close();
                    })
                    .catch(function (error) {
                        alert(error);
                    });

            }
        },
        async created() {
            await this.getIclaimApproval();
        },
        //async mounted() {
            

        //}

    }
</script>