<template>
    <div class="container ">
        <div class="row">
            <h2 id="header2">ยืนยันตัวตน</h2>
        </div>
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
                               v-bind:allow-multiple="false"
                               v-bind:allowFileEncode="true"
                               accepted-file-types="image/jpeg, image/png"
                               v-bind:files="idCardFile"
                               v-on:addfile="onAddidCardFile" />
                </div>
                <div class="col-6">
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

        </div>
        <div class="row">
            <div class="col-12">
                <router-link class="btn-next" style="width: 100%; padding: 8px 0px;" to="/Accident">ดำเนินการต่อ</router-link>
            </div>
        </div>




    </div>
</template>

<script>


    import qs from 'qs'
    import sha256 from 'crypto-js/sha256'
    import hmacSHA256 from 'crypto-js/hmac-sha256'

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
                dataIdCard: {},
                scoreCompare: {},
                idCardFile: null,
                faceFile: null,
                idCardTest: { file: null, filename: "", base64: "" },
                faceTest: { file: null, filename: "", base64: "" },
                SECRET_ID: "cc425efa2ec80be4c1149f0642c9ff94",
                SECRET_KEY: "d387810e33e19128c8cc6643c7758143408d6359",
                URI: "/v1/compare-face-id-card",
                URL: "https://iai.flashsoftapi.com/v1/compare-face-id-card"
            };
        },

        methods: {
            encode_utf8(s) {
                return unescape(encodeURIComponent(s));
            },
            generate_nonce() {
                let nonce = '';
                for (let num = 1; num <= 16; num++) {
                    nonce += Math.floor(Math.random() * 10).toString()
                }
                return nonce;
            },
            current_timestamp() {
                let now = parseInt(new Date().getTime() / 1000);
                return now.toString();
            },
            get_date(timestamp) {
                let now = new Date(parseInt(timestamp) * 1000);
                now.setMinutes(now.getMinutes() + now.getTimezoneOffset());
                let yy = now.getFullYear();
                let mm = now.getMonth() + 1;
                if (String(mm).length !== 2) {
                    mm = '0' + mm
                }
                let dd = now.getDate();
                if (String(dd).length !== 2) {
                    dd = '0' + dd
                }
                return yy + "-" + mm + "-" + dd;
            },
            generate_token_readIdCard(secret_id, secret_key, timestamp, data) {
                let host = 'iai.flashsoftapi.com';
                let today = this.get_date(timestamp);

                let payload = data;
                let digest_data = sha256(payload);

                let canonical_request = "POST" + "\n" + "/v1/thai-id-card-ocr" + "\n\n" + "content-type:application/x-www-form-urlencoded" + "\n" + "host:" + host + "\n\n" + "content-type;host" + "\n";
                let digest = sha256(canonical_request + digest_data);

                let string_to_sign = "FC1-HMAC-SHA256" + "\n" + timestamp + "\n" + today + "/th/fc1_request" + "\n" + digest
                let secretkey = "FC1" + secret_key;
                let v1 = this.encode_utf8(secretkey.toString());
                let v2 = this.encode_utf8(today.toString());
                let secret_date = hmacSHA256(v2, v1);
                let secret_service = hmacSHA256('th', secret_date);
                let secret_signing = hmacSHA256('fc1_request', secret_service);
                let signature = hmacSHA256(string_to_sign, secret_signing).toString();
                return "FC1-HMAC-SHA256" + " " + "Credential=" + secret_id.toString() + "/" + today + "/th/fc1_request" + ", " + "SignedHeaders=content-type;host" + ", " + "Signature=" + signature.toString();
            },
            generate_token(secret_id, secret_key, uri, timestamp, data) {
                let host = 'iai.flashsoftapi.com';
                let today = this.get_date(timestamp);

                let payload = data;
                let digest_data = sha256(payload);

                let canonical_request = "POST" + "\n" + uri.toString() + "\n\n" + "content-type:application/x-www-form-urlencoded" + "\n" + "host:" + host + "\n\n" + "content-type;host" + "\n";
                let digest = sha256(canonical_request + digest_data);

                let string_to_sign = "FC1-HMAC-SHA256" + "\n" + timestamp + "\n" + today + "/th/fc1_request" + "\n" + digest
                let secretkey = "FC1" + secret_key;
                let v1 = this.encode_utf8(secretkey.toString());
                let v2 = this.encode_utf8(today.toString());
                let secret_date = hmacSHA256(v2, v1);
                let secret_service = hmacSHA256('th', secret_date);
                let secret_signing = hmacSHA256('fc1_request', secret_service);
                let signature = hmacSHA256(string_to_sign, secret_signing).toString();
                return "FC1-HMAC-SHA256" + " " + "Credential=" + secret_id.toString() + "/" + today + "/th/fc1_request" + ", " + "SignedHeaders=content-type;host" + ", " + "Signature=" + signature.toString();
            },
            onAddidCardFile: async function (error, file) {
                this.idCardTest.file = file
                this.idCardTest.filename = file.getFileEncodeDataURL()
                this.idCardTest.base64 = file.getFileEncodeBase64String()
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
                await this.readIdCard()


            },
            onAddfaceFile: async function (error, file) {
                this.faceTest.file = file
                this.faceTest.filename = file.getFileEncodeDataURL()
                this.faceTest.base64 = file.getFileEncodeBase64String()
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
                await this.demo_url()

            },
            readIdCard() {
                var url = "https://sv1.picz.in.th/images/2021/08/19/2vfhUf.jpg";
                let data = qs.stringify({
                    'url': url,
                });
                let timestamp = this.current_timestamp();
                let token = this.generate_token_readIdCard("681eac5be10f3632cd659cfc7d87380e", "86e6d2be10a59432ac1528b684224101b59313c2", timestamp, data);
                let config = {
                    method: 'post',
                    url: 'https://iai.flashsoftapi.com/v1/thai-id-card-ocr',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'Authorization': token,
                        'X-FC-NONCE': this.generate_nonce(),
                        'X-FC-TIMESTAMP': timestamp,
                    },
                    data: data
                };

                axios(config)
                    .then((response) => {
                        this.dataIdCard = response.data.result;
                        console.log(this.dataIdCard);
                        if (this.dataIdCard != "") {
                            this.$swal.close();

                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });

            },
            demo_url() {

                var url_a = "https://sv1.picz.in.th/images/2021/08/19/2vfhUf.jpg";
                var url_b = "https://sv1.picz.in.th/images/2021/08/19/2vfTcz.jpg";

                let data = qs.stringify({
                    'id_card_url': url_a,
                    'selfie_url': url_b,
                });
                let timestamp = this.current_timestamp();
                let token = this.generate_token(this.SECRET_ID, this.SECRET_KEY, this.URI, timestamp, data);
                let config = {
                    method: 'post',
                    url: this.URL,
                    headers: {
                        'Access-Control-Allow-Origin': '*',
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'Authorization': token,
                        'X-FC-NONCE': this.generate_nonce(),
                        'X-FC-TIMESTAMP': timestamp,
                    },
                    data: data
                };

                axios(config)
                    .then((response) => {
                        this.scoreCompare = response.data.result;
                        console.log(this.scoreCompare);
                        if (this.scoreCompare != "") {
                            this.$swal.close();
                            this.$swal({
                                icon: 'success',
                                text: 'ผลการเทียบใบหน้ากับบัตรประจำตัวประชาชนตรงกัน ' + this.scoreCompare.score + "%",
                                title: 'ตรวจสอบสำเร็จ',
                                /*footer: '<a href="">Why do I have this issue?</a>'*/
                                showCancelButton: false,
                                showDenyButton: false,

                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                            })
                        }
                        //   console.log('result: ' +  JSON.stringify(data.result, null, 4));
                        //   console.log('request_id: ' + data.request_id); // for debug use
                    })
                    .catch(function (error) {
                        console.log(error.response.data);
                    });

            }



        },
        mounted() {


        }
    };
</script>

<style>
</style>